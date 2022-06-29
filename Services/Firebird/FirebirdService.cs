using FirebirdSql.Data.FirebirdClient;
namespace parser_cont.Services.Firebird;
public static class FirebirdService
{
    private const string StringConnection = "database=10.0.0.23:Cont;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;";
    
    #region Main Service

     // Попытка привести к единому формату
    private static string ReplaceCitizen(string s)
    {
        StringBuilder sb = new(s);
        // Украина
        sb.Replace("Україна", "UA");
        sb.Replace("Украина", "UA");
        sb.Replace("украинское", "UA");
        sb.Replace("УКРАИНА", "UA");
        // Россия
        sb.Replace("Російська федерація", "RU");
        sb.Replace("РФ", "RU");
        sb.Replace("Российская Федерация", "RU");
        // ЛНР
        sb.Replace("ЛНР", "LNR");
        sb.Replace(".ЛНР", "LNR");
        sb.Replace("Лнр", "LNR");
        sb.Replace("Луганская народная республика", "LNR");
        // ДНР
        sb.Replace("ДНР", "DNR");
        return sb.ToString().Trim();
    }
    private static string ReplaceOckenka(string s)
    {
        StringBuilder sb = new(s);
        //Тип предмета
        // A - предмет, по которому нет контроля в данном семестре 
        // B - государственный экзамен
        // D - защита квалификационной работы 
        // E - екзамен
        // K - курсовая работа
        // N - дифференцированный зачет, который не учитывается в рейтинге и при выдаче стипендии
        // P - практика
        // Z - зачет
        sb.Replace("B", "государственный экзамен");
        sb.Replace("D", "защита квалификационной работы");
        sb.Replace("E", "экзамен");
        sb.Replace("K", "курсовая работа");
        sb.Replace("N", "дифференцированный зачет");
        sb.Replace("P", "практика");
        sb.Replace("Z", "зачет");
        return sb.ToString().Trim();
    }
    private static string ReplaceLevel(string s)
    {
        StringBuilder sb = new(s);
        sb.Replace("Магистр", "Магистратура");
        sb.Replace("Бакалавр", "Бакалавриат");
        return sb.ToString().Trim();
    }
    private static string FirstCharToUpper(this string input) => input switch
    {
        null => throw new ArgumentNullException(nameof(input)),
        "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
    };
    public static async Task<IEnumerable<StudentOcenka>> GetStudentMarks(int groupId)
    {
        var list = new List<StudentOcenka>();
        var sql =
            $" select s.id from student s inner join stud_gruppa sg on s.id = sg.stud_id  inner join gruppa g on g.id = sg.grup_id  where g.is_vip = 'F' and g.id = {groupId}";
        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new StudentOcenka
            {
                Id = reader.GetInt32(0),
                Plans = await GetPlan(reader.GetInt32(0))
            });

        }
        await reader.CloseAsync();

        return list;
    }
    public static async Task<IEnumerable<Plan>> GetPlan(int idStudent)
    {
        var list = new List<Plan>();
        var sql =
            "select distinct up.name, up.semestr , up.typ , o.ball, o.ocenka, o.ocenka_ects " +
            "from student s " +
            "inner join stud_gruppa sg on s.id = sg.stud_id " +
            "inner join ocenky o on o.stud_id = s.id " +
            "inner join uch_plan up on up.id = o.up_id " +
            "where s.id = " + idStudent;

        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string marks;
            if (reader.GetString(2).Trim() == "Z")
            {
                if (reader.GetString(4) == "2" || reader.GetString(4) == "0")
                    marks = "0";
                else
                    marks = "1";
            }
            else
                marks = reader.GetString(4);

            list.Add(new Plan
            {
                SubjectName = reader["name"] != DBNull.Value ? reader.GetString(0).Trim() : "не указано",
                Semester = reader["semestr"] != DBNull.Value ? reader.GetString(1) : "0",
                FormControl = ReplaceOckenka(reader.GetString(2)),
                Ball_100 = reader["ball"] != DBNull.Value ? reader.GetInt32(3) : 0,
                Ball_5 = marks,
                Ball_ECTS = reader.GetString(5).Trim(),
            });
        }

        await reader.CloseAsync();

        return list;
    }
    public static async Task<IEnumerable<Specialtys>> GetNewSpecialty()
    {
        var list = new List<Specialtys>();
        const string sql = " select s.name as specialty ,s.id, s.nick,   s.min_id , s.prof_podg  , sl.name as st_level ,fo.name as fo_name, fak.name as fak_name" +
                           " from specialnost s"
                           + " inner join gruppa g on g.spec_id = s.id"
                           + " inner join ST_LEVELS sl on sl.id = g.st_lvl_id"
                           + " inner join form_obuch fo on fo.id = g.fo_id"
                           + " inner join fakultet fak on fak.id = g.fak_id"
                           + " where g.is_vip = 'F' and fak.id NOT IN (9,15,2,24,19,11,25,30, 18) ";
        
        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Specialtys
            {
                IdSpecialty = reader.GetInt32(1),
                NameSpecialty = reader.GetString(0).ToLower().Trim(),
                ShortSpecialty = reader["nick"] != DBNull.Value ? reader.GetString(2).ToLower().Trim() : "Не указано",
                MinId = reader["min_id"] != DBNull.Value && string.IsNullOrWhiteSpace(reader.GetString(3)) ? reader.GetString(3).ToLower().Trim() : "Не указано",
                Profile = reader["prof_podg"] != DBNull.Value ? reader.GetString(4).ToLower().Trim() : null,
                Level = ReplaceLevel(FirstCharToUpper(reader.GetString(5).Trim().ToLower())),
                Form = FirstCharToUpper(reader.GetString(6).Trim().ToLower()),
                NameDepartment = reader.GetString(7).Trim() == "Колледж Луганского государственного педагогического университета"
                    ? "ОП «Многопрофильный педагогический колледж ЛГПУ»"
                    : reader.GetString(7).Trim()

            });
            
        }
        await reader.CloseAsync();

        return list;
    }
    private static async Task<IEnumerable<Groups>> GetGroups(int idStudent, int idFacult)
    {
        var list = new List<Groups>();
        var sql =
            $" select  G.id, G.NAME, G.KURS, G.GOD_OBR,  ST.NAME as LEVELS , FR.NAME as FORM , FF.NAME as FKNAME, SG.IS_BUDG, SG.N_ZACH , G.SPEC_ID , P.NAME as ORDER_NAME , P.DATE_CRT as DATE_ORDER , TP.NAME as TYPE_ORDER  " +
            $" from stud_gruppa SG  " +
            $" inner join gruppa G on SG.GRUP_ID = G.id  " +
            $" inner join ST_LEVELS ST on ST.id = G.ST_LVL_ID  inner join FORM_OBUCH FR on FR.id = G.FO_ID  " +
            $" inner join FAKULTET FF on FF.id = G.FAK_ID  " +
            $" inner join PRIKAZ P on P.id = SG.PRIKAZ_ID  " +
            $" inner join TYP_PRIKAZ TP on TP.id = P.TYP " +

            $" where SG.STUD_ID = {idStudent} " +
            $" and G.FAK_ID = {idFacult}" +
            $" and G.IS_VIP = 'F' ";

        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var idGroup = reader.GetInt32(0);

            var typeOrder = (string)reader["IS_BUDG"] == "T" && reader.GetString(12) == "зачисление"
                ? "зачисление на бюджетное место"
                : "зачисление на контрактное место";
            list.Add(new Groups
            {
                IdGroup = idGroup,
                NameGroup = reader.GetString(1).Trim(),
                Course = reader["KURS"] != DBNull.Value ? reader.GetInt16(2) : 0,
                YearStart = reader["GOD_OBR"] != DBNull.Value ? reader.GetString(3) : "0",
                Level = reader["LEVELS"] != DBNull.Value ? reader.GetString(4) : "",
                Form = reader["FORM"] != DBNull.Value ? reader.GetString(5) : "",
                Faculty = reader["FKNAME"] != DBNull.Value ? reader.GetString(6) : "",
                //Basis = (string)reader["IS_BUDG"] == "T" ? "бюджет" : "контракт",
                RecordBook = reader["N_ZACH"] != DBNull.Value ? reader.GetString(8) : "Не указано",// Зачетная книга
                idSpecialty = reader.GetInt32(9), // Код специальности
                orderName = reader["ORDER_NAME"] != DBNull.Value ? reader.GetString(10) : "Не указано",
                orderDate = reader["DATE_ORDER"] != DBNull.Value ? reader.GetDateTime(11).ToString("yyyy-MM-dd") : "2022-05-16",
                orderType = typeOrder,
                //Specialty = await GetSpesialties(idGroup)
            });
        }

        await reader.CloseAsync();

        return list;
    }
    public static async Task<IEnumerable<Students>> GetStudentsAll()
    {
        List<Students> list = new();

        const string sqlGrid = " select " +
                               " distinct " +
                               " S.id," + //0
                               " S.FAMIL," +//1
                               " S.NAME," +//2
                               " S.OTCH," +//3
                               " S.D_BIRTH," +//4
                               " S.IS_MALE," +//5
                               " S.CITIZEN," +//6
                               " S.IND_KOD," +//7
                               " S.TEL_DOM," +//8
                               " S.TEL_MOB," +//9
                               " S.TEL_THIRD," +//10
                               " S.ADRES_PR," +//11
                               " S.ADRES_F," +//12
                               " S.SER_PASP," +//13
                               " S.N_PASP," +//14
                               " S.KEM_VIDAN_PASP," + //15
                               " S.D_VIDACHI_PASP," +//16
                               " S.TIP_DOC," +//17
                               " S.IS_OBSHAGA," +//18
                               " S.IS_TREB_OBSH," +//19
                               //  " S.ABID," +//20
                               //   " DOP.OBR_ZAV," +//20
                               //   " SG.GRUP_ID as GroupId, " +
                               //   " G.NAME as GROUPNAME, " +
                               //   " SG.god_post," +
                               //   " SG.IS_BUDG," +
                               //   " SG.N_ZACH, " +
                               " S.MESTO_ROGD ," + //21
                               " G.FAK_ID " + //22
                               " from STUDENT S " +
                               " inner join stud_gruppa SG on SG.stud_id = S.id " +
                               " inner join gruppa G on SG.GRUP_ID = G.id " +
                               $" where G.IS_VIP = 'F' and G.fak_id in (1, 3, 4, 5, 7, 8 , 13 ,28, 36)" +
                               " order by S.FAMIL asc";

        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sqlGrid, connection, transaction);
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var idStudent = reader.GetInt32(0);
            var idFaculty = reader.GetInt32(21);

            list.Add(new Students
            {
                IdStudent = idStudent,
                FirstName = reader.GetString(2),
                MiddleName = reader.GetString(3),
                LastName = reader.GetString(1),
                Birthday = reader["D_BIRTH"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : "2022-05-16",
                Gender = (string)reader["IS_MALE"] == "T" ? "male" : "female",  // Авто замена данных,
                Citizen = reader["CITIZEN"] != DBNull.Value ? ReplaceCitizen(reader.GetString(6)) : "Не указано",
                Code = reader["IND_KOD"] != DBNull.Value ? reader.GetString(7) : "Не указано",
                FirstPhone = reader["TEL_DOM"] != DBNull.Value ? reader.GetString(8) : "Не указано",
                SecondPhone = reader["TEL_MOB"] != DBNull.Value ? reader.GetString(9) : "Не указано",
                ThirdPhone = reader["TEL_THIRD"] != DBNull.Value ? reader.GetString(10) : "Не указано",
                AdressRegistration = reader["ADRES_PR"] != DBNull.Value ? reader.GetString(11).Trim() : "Не указано", // Прописка
                AdressActual = reader["ADRES_F"] != DBNull.Value ? reader.GetString(12) : "Не указано",
                SerialPassport = reader["SER_PASP"] != DBNull.Value ? reader.GetString(13) : "Не указано",
                NumberPassport = reader["N_PASP"] != DBNull.Value ? reader.GetString(14) : "Не указано",
                OrganizationPassport = reader["KEM_VIDAN_PASP"] != DBNull.Value ? reader.GetString(15) : "Не указано",
                DatePassport = reader["D_VIDACHI_PASP"] != DBNull.Value ? reader.GetDateTime(16).ToString("yyyy-MM-dd") : "2022-05-16",
                TypeDocument = reader["TIP_DOC"] != DBNull.Value ? reader.GetString(17).Replace(" ", "").ToString().Trim().ToLower() : "паспорт",
                IsHostel = reader["IS_OBSHAGA"] != DBNull.Value && (reader.GetString(18) == "T"), // if == T = true else false
                NeedHostel = reader["IS_TREB_OBSH"] != DBNull.Value && (reader.GetString(19) == "T"),
                BirthPlace = reader["MESTO_ROGD"] != DBNull.Value ? reader.GetString(20) : "Не указано",

                // Записываем в группы
                Groups = await GetGroups(idStudent, idFaculty),
                // 
                OrganizationEducation = await GetEducation(idStudent),
                // Родители
                Relatives = await GetRelatives(idStudent)

            });
        }
        await reader.CloseAsync();

        return list;
    }
    public static async Task<IEnumerable<Students>> GetStudents(string idFak)
        {
            var list = new List<Students>();

            var sqlGrid =
                " select " +
                " distinct " +
                " S.id," + //0
                " S.FAMIL," +//1
                " S.NAME," +//2
                " S.OTCH," +//3
                " S.D_BIRTH," +//4
                " S.IS_MALE," +//5
                " S.CITIZEN," +//6
                " S.IND_KOD," +//7
                " S.TEL_DOM," +//8
                " S.TEL_MOB," +//9
                " S.TEL_THIRD," +//10
                " S.ADRES_PR," +//11
                " S.ADRES_F," +//12
                " S.SER_PASP," +//13
                " S.N_PASP," +//14
                " S.KEM_VIDAN_PASP," + //15
                " S.D_VIDACHI_PASP," +//16
                " S.TIP_DOC," +//17
                " S.IS_OBSHAGA," +//18
                " S.IS_TREB_OBSH," +//19
                                    //  " S.ABID," +//20
                                    //   " DOP.OBR_ZAV," +//20
                                    //   " SG.GRUP_ID as GroupId, " +
                                    //   " G.NAME as GROUPNAME, " +
                                    //   " SG.god_post," +
                                    //   " SG.IS_BUDG," +
                                    //   " SG.N_ZACH, " +
                " S.MESTO_ROGD " + //21
                " from STUDENT S " +
                " inner join stud_gruppa SG on SG.stud_id = S.id " +
                " inner join gruppa G on SG.GRUP_ID = G.id " +
                $" where G.FAK_ID = {idFak} and G.IS_VIP = 'F' " +
                " order by S.FAMIL asc";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sqlGrid, connection, transaction);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var idStudent = reader.GetInt32(0);

                list.Add(new Students
                {
                    IdStudent = idStudent,
                    FirstName = reader.GetString(2),
                    MiddleName = reader.GetString(3),
                    LastName = reader.GetString(1),
                    Birthday = reader["D_BIRTH"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : "2022-05-16",
                    Gender = (string)reader["IS_MALE"] == "T" ? "male" : "female",  // Авто замена данных,
                    Citizen = reader["CITIZEN"] != DBNull.Value ? ReplaceCitizen(reader.GetString(6)) : "Не указано",
                    Code = reader["IND_KOD"] != DBNull.Value ? reader.GetString(7) : "Не указано",
                    FirstPhone = reader["TEL_DOM"] != DBNull.Value ? reader.GetString(8) : "Не указано",
                    SecondPhone = reader["TEL_MOB"] != DBNull.Value ? reader.GetString(9) : "Не указано",
                    ThirdPhone = reader["TEL_THIRD"] != DBNull.Value ? reader.GetString(10) : "Не указано",
                    AdressRegistration = reader["ADRES_PR"] != DBNull.Value ? reader.GetString(11).Trim() : "Не указано", // Прописка
                    AdressActual = reader["ADRES_F"] != DBNull.Value ? reader.GetString(12) : "Не указано",
                    SerialPassport = reader["SER_PASP"] != DBNull.Value ? reader.GetString(13) : "Не указано",
                    NumberPassport = reader["N_PASP"] != DBNull.Value ? reader.GetString(14) : "Не указано",
                    OrganizationPassport = reader["KEM_VIDAN_PASP"] != DBNull.Value ? reader.GetString(15) : "Не указано",
                    DatePassport = reader["D_VIDACHI_PASP"] != DBNull.Value ? reader.GetDateTime(16).ToString("yyyy-MM-dd") : "2022-05-16",
                    TypeDocument = reader["TIP_DOC"] != DBNull.Value ? reader.GetString(17).Replace(" ", "").ToString().Trim().ToLower() : "паспорт",
                    IsHostel = reader["IS_OBSHAGA"] != DBNull.Value && (reader.GetString(18) == "T"), // if == T = true else false
                    NeedHostel = reader["IS_TREB_OBSH"] != DBNull.Value && (reader.GetString(19) == "T"),
                    BirthPlace = reader["MESTO_ROGD"] != DBNull.Value ? reader.GetString(20) : "Не указано",
                    // Записываем в группы
                    Groups = await GetGroups(idStudent, int.Parse(idFak)),
                    // 
                    OrganizationEducation = await GetEducation(idStudent),
                    // Родители
                    Relatives = await GetRelatives(idStudent)

                });
            }
            await reader.CloseAsync();

            return list;
        }
    public static async Task<IEnumerable<OrganizationEducation>> GetEducation(int idStudent)
        {
            List<OrganizationEducation> list = new();
            var sqlGrid = $"select OBR_ZAV from DOP_OBUCH  where STUD_ID = {idStudent}";
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sqlGrid, connection, transaction);
            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new OrganizationEducation
                {
                    Name = reader.GetString(0),
                });

            }
            await reader.CloseAsync();

            return list;
        }
    public static async Task<IEnumerable<Relatives>> GetRelatives(int idStudent)
        {
            List<Relatives> list = new();
            var sql =
                $" select ROD.FIO, TYP.NAME, ROD.ADRES, ROD.TEL_DOM, ROD.TEL_RAB  from RODITELI ROD  left join TYP_RODSTVO TYP on TYP.ID = ROD.RODSTVO_ID  where ROD.STUD_ID = {idStudent}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Relatives
                {
                    FullName = reader["FIO"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    Type = reader["NAME"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                    Adress = reader["ADRES"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    Mobbile = reader["TEL_DOM"] != DBNull.Value ? reader.GetString(3) : "Не указано",
                    ExtraMobbile = reader["TEL_RAB"] != DBNull.Value ? reader.GetString(4) : "Не указано",
                });
            }
            await reader.CloseAsync();

            return list;
        }
    public static async Task<IEnumerable<Spesialty>> GetSpesialties(int idGroup)
    {
            List<Spesialty> list = new();
            var sql =
                $"select  S.id, S.NAME, S.NICK, S.MIN_ID  from SPECIALNOST S inner join gruppa G on S.id = G.SPEC_ID  where G.id = {idGroup} and G.IS_VIP = 'F'";


            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Spesialty
                {
                    IdSpecialty = reader.GetInt32(0),
                    Name = reader["NAME"] != DBNull.Value ? reader.GetString(1).Trim().ToLower() : "не указано",
                    Nick = reader["NICK"] != DBNull.Value ? reader.GetString(2) : "не указано",
                    MinId = reader["MIN_ID"] != DBNull.Value ? reader.GetString(3) : "не указано"

                });
            }

            await reader.CloseAsync();

            return list;
    }

    #endregion
    
}

