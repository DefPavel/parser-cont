using FirebirdSql.Data.FirebirdClient;

namespace parser_cont.Services.Firebird;

public static class FirebirdServicePersonnel
{
    private const string StringConnection = "database=10.0.0.23:Pers;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;";
        private static int _num;
        private static string FirstCharToUpper(this string input) => input switch
       {
           null => throw new ArgumentNullException(nameof(input)),
           "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
           _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
       };

        #region Список должностей на отдел

        private static async Task<IEnumerable<Position>> GetPositionsAsync(int idDep)
        {
            // Через Массивы намного быстрее идет заполнение 
            // List - 7 сек.
            // Array - 3 сек.
            var array = new List<Position>();
            var sql =
                $" select distinct D.NAME, D.IS_PED,D.OKLAD_B, D.OKLAD_NB,D.OTPUSK,D.KOLVO_B,D.KOLVO_NB,D.FREE_B,D.FREE_NB,D.PRIORITI_DOLJ,D.NAME_ROD  from DOLJNOST D where D.OTDEL_ID = {idDep}";

        await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                // Поднимаем первую букву 
                var namePosition = FirstCharToUpper(reader.GetString(0).Trim());

                array.Add(new Position
                {
                    Name = namePosition,
                    IsPed = (string)reader["IS_PED"] == "T",
                    oklad_B = reader["OKLAD_B"] != DBNull.Value ? reader.GetDecimal(2) : 0,
                    oklad_N = reader["OKLAD_NB"] != DBNull.Value ? reader.GetDecimal(3) : 0,
                    holidayLimit = (short)(reader["OTPUSK"] != DBNull.Value ? reader.GetInt16(4) : 0),
                    count_B = reader["KOLVO_B"] != DBNull.Value ? reader.GetDecimal(5) : 0,
                    count_NB = reader["KOLVO_NB"] != DBNull.Value ? reader.GetDecimal(6) : 0,
                    free_b = reader["FREE_B"] != DBNull.Value ? reader.GetDecimal(7) : 0,
                    free_nb = reader["FREE_NB"] != DBNull.Value ? reader.GetDecimal(8) : 0,
                    priority = (short)(reader["PRIORITI_DOLJ"] != DBNull.Value ? reader.GetInt16(9) : 0),
                    padeg = reader["NAME_ROD"] != DBNull.Value ? reader.GetString(10).Trim() : "Не указано"
                });
            }
            return array.AsReadOnly();
        }
        #endregion

        #region Список отделов
        public static async Task<IEnumerable<Department>> GetDepartment()
        {
            var array = new List<Department>();
            const string sql = $" select * from get_tree_root";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var typeDep = reader.GetString(3).ToLower().Contains("институт") ? "Институт" :
                reader.GetString(3).ToLower().Contains("кафедра") ? "Кафедра" :
                reader.GetString(3).ToLower().Contains("отдел") ? "Отдел" :
                reader.GetString(3).ToLower().Contains("факультет") ? "Факультет" :
                reader.GetString(3).ToLower().Contains("ректорат") ? "Ректорат" :
                reader.GetString(3).ToLower().Contains("центр") ? "Центр" :
                reader.GetString(3).ToLower().Contains("сектор") ? "Сектор" :
                reader.GetString(3).ToLower().Contains("колледж") ? "Колледж" :
                reader.GetString(3).ToLower().Contains("центр") ? "Центр" :
                "Не указано";
                if (typeDep == null) throw new ArgumentNullException(nameof(typeDep));

                array.Add(new Department
                {
                    Id = reader.GetInt32(1),
                    Phone = reader["phone"] != DBNull.Value ? reader.GetString(2).Trim() : "Не указано",
                    Name = reader.GetString(3).Trim(),
                    Short = reader.GetString(4).Trim(),
                    Padeg = reader.GetString(5).Trim(),
                    Type = typeDep,
                    Root = reader.GetString(7),
                    Positions = await GetPositionsAsync(reader.GetInt32(1))
                });
              
            }
            await reader.CloseAsync();

            return array.AsReadOnly();
        }

        #endregion

        #region Основная информация по сотрудникам
        public static async Task<int> GetCountPersons()
        {
            const string sql =
                "select count (distinct s.id) from sotr s inner join sotr_doljn sd on s.id = sd.sotr_id  where sd.dolj_id <> 0 ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            return reader.Read() ? reader.GetInt32(0) : 0;
        }
    public static async Task<int> GetCountDropPersons()
    {
        const string sql =
            "select count (distinct s.id) from sotr s inner join sotr_doljn sd on s.id = sd.sotr_id  where sd.dolj_id = 0 ";

        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        await using var reader = await command.ExecuteReaderAsync();
        return reader.Read() ? reader.GetInt32(0) : 0;
    }
    public static async Task<int> GetCountAllPersons()
    {
        const string sql =
            "select count (distinct s.id) from sotr s inner join sotr_doljn sd on s.id = sd.sotr_id ";

        await using FbConnection connection = new(StringConnection);
        connection.Open();
        await using var transaction = await connection.BeginTransactionAsync();
        await using FbCommand command = new(sql, connection, transaction);
        await using var reader = await command.ExecuteReaderAsync();
        return reader.Read() ? reader.GetInt32(0) : 0;
    }
    public static async Task<IEnumerable<Persons>> GetPersonsAsync(int first = 100 , int skip = 0)
    {
            var array = new List<Persons>();
        string sql = "select " +
                           $" first {first} " +
                           $" skip {skip}" +
                           " distinct " +
                           "s.famil ," + //0
                           "s.name ," +//1
                           "s.otch ," +//2
                           "s.sex ," +//3
                           "s.date_birth ," +//4
                           "s.IDENT_CODE," +//5
                           "s.phone," + // 6
                           "s.phone_lug," +// 7
                           "s.address," + //8
                           "s.pasp_typ," +//9
                           "s.pasp_ser," +//10
                           "s.pasp_n ," +//11
                           "s.pasp_date," +//12
                           "s.pasp_organ ," +//13
                           "s.pasp_rus," + //14
                           "s.ROD_STAN," +//15
                           "s.is_stud," +//16
                           "s.is_aspirant," +//17
                           "s.is_outher," +//18
                           "s.is_doktor," +//19
                           "s.is_rus_snils," +//20
                           "s.mat_otv," +//21
                           "s.MAT_ODINOCH," +//22
                           "s.MAT_DVOIH," +//23
                           "s.PREVIOUS_CONVICTION, " +//24
                           "s.date_start_job," +//25
                           "s.FIO_DATEL  ," +//26
                           "s.id ," +//27
                                     //"s.photo, " +
                           "s.prim " +
                           "from sotr s " +
                           " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                           //" where  sd.dolj_id <> 0 " +
                              " where s.pasp_n is not null and s.date_birth <> '12.02.2011 11:02:21'  " +
                             //" and s.id = 5547 " + 
                             " and s.date_birth <> '12.02.2011 11:02:20' and s.id <> 5661 and s.id <> 5229 and s.id <> 4948 and s.id <> 4822 and s.id <> 3674 and s.id <> 4518 and s.id <> 75 and s.id <> 3981 and s.id <> 4154 and s.id <> 1955 and s.id <>2214 and s.id <> 5229 and s.id <> 4948 and s.id <> 226 and s.id <> 4576 and s.id <> 541 and s.id <> 573 and s.id <> 3520 and s.id <> 697 and s.id <> 4822 and s.id <> 990 and s.id <> 851 and s.id <> 953 and s.id <> 2672 and s.id <> 999 and s.id <> 1098 and s.id <> 1146 and s.id <> 1156 and s.id <> 1328 and s.id <> 1388 and s.id <> 1397 and s.id <> 2848 and s.id <> 1508 " + // Дубликаты 
                             " order by s.id desc ";

        await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            var i = 0;
            while (await reader.ReadAsync())
            {
                //byte[] temp_backToBytes = Convert.FromBase64String((byte[])reader["photo"]);

                var isNum = int.TryParse(reader.GetString(10).Replace(" ", ""), out _num);
                string typePassport;
                if (isNum)
                {
                    typePassport = "Паспорт(России)";
                }
                else
                {
                    typePassport = reader.GetString(10).Replace(" ", "") == "ТН" ? "Паспорт(ЛНР)" :
                        reader.GetString(10).Replace(" ", "") == "ТМ" ? "Паспорт(ЛНР)" :
                        reader.GetString(10).Replace(" ", "") == "ТТ" ? "Паспорт(ЛНР)" :
                        reader.GetString(10).Replace(" ", "") == "нет" ? "Не указано" :
                        "Паспорт(Украины)";
                }

            //Внешний совместитель
                array.Add(new Persons
                {
                    persId = reader.GetInt32(27),
                    FirstName = reader.GetString(0).Trim(),
                    Name = reader.GetString(1).Trim(),
                    LastName = reader.GetString(2).Trim(),
                    Gender = reader.GetString(3) == "Жен" ? "female" : "male",
                    DateBirthay = reader["date_birth"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : "1970-01-01",
                    Code = reader["IDENT_CODE"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                    //photo = reader["photo"] as byte[] != null ? Convert.ToBase64String(reader["photo"] as byte[]) : null,
                    PhoneUA = reader.GetString(6),
                    PhoneLG = reader.GetString(7),
                    Adress = reader.GetString(8).Trim(),
                    TypePassport = typePassport,
                    SerialPassport = reader["pasp_ser"] != DBNull.Value ? reader.GetString(10) : "Не указано",
                    NumberPassport = reader["pasp_n"] != DBNull.Value ? reader.GetString(11) : "Не указано",
                    DatePassport = reader["pasp_date"] != DBNull.Value ? reader.GetDateTime(12).ToString("yyyy-MM-dd") : "1970-01-01",
                    OrganiztionPassport = reader.GetString(13),
                    IsRusPassport = reader.GetString(14) == "T", // Имеет ли российский паспорт
                    IsMarriage = reader.GetString(15) == "T", // Состоит ли в браке
                    IsStudent = reader.GetString(16) == "T",
                    IsAspirant = reader.GetString(17) == "T",
                    IsDoctor = reader.GetString(19) == "T",
                    IsSnils = reader.GetString(20) == "T", // Снилс
                    IsResponsible = reader.GetString(21) == "True", // Матереально отвест.
                    IsSingleMother = reader.GetString(22) == "T",
                    IsTwoChildMother = reader.GetString(23) == "T",
                    IsPreviosConvition = reader.GetString(24) == "T",// Справка о не судимости
                    DateToWorking = reader["date_start_job"] != DBNull.Value ? reader.GetDateTime(25).ToString("yyyy-MM-dd") : "1970-01-01",
                    Padeg = reader["FIO_DATEL"] != DBNull.Value ? reader.GetString(26) : "Не указано",
                    description = reader["prim"] != DBNull.Value ? reader["prim"].ToString() : null,
                    ArrayPositions = await GetPersonPosition(reader.GetInt32(27)), // Должности
                    ArraySurname = await GetChangeSurname(reader.GetInt32(27)),// Смена фамилии
                    ArrayFamily = await GetFamiliesAsync(reader.GetInt32(27)), // Родственники

                    ArrayPensioners = await GetPensionerAsync(reader.GetInt32(27)), // Пенсонеры
                    ArrayInvalids = await GetInvalids(reader.GetInt32(27)), //Инвалиды

                    ArraEducation = await GetEducationsAsync(reader.GetInt32(27)), // Образование
                    ArrayHistoryBook = await GetHistoryBooksAsync(reader.GetInt32(27)),// Трудовая книга
                    ArrayMedical = await GetMedicals(reader.GetInt32(27)), // Медицинское обзраование
                                                                            // ArrayMove = await GetMovesAsync(reader.GetInt32(27)),
                    ArrayAcademics = await ChlenAcademicsAsync(reader.GetInt32(27)),
                });
            i++;
            }
            Console.WriteLine(i);
            return array.AsReadOnly();
        }

        private static async Task<IEnumerable<ChlenAcademic>> ChlenAcademicsAsync(int idPers)
        {
            var list = new List<ChlenAcademic>();

            var sql =
                $" select distinct chl.name , chl.academ_name, chl.num_diploma , chl.obr_date from sotr_chlen_academ chl where chl.sotr_id ={idPers}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new ChlenAcademic
                {
                    Type = reader.GetString(0),
                    AcademicalName = reader["name"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                    NumberDiplom = reader["num_diploma"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    Date = reader["obr_date"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                });
            }
            return list;
        }

        private static async Task<IEnumerable<Medical>> GetMedicals(int idPers)
        {
            var list = new List<Medical>();

            var sql =
                $" select distinct pm.text , mk.name , pm.date_start , pm.date_finish from pers_med pm inner join med_kategor mk on mk.id = pm.kat_id where pm.sotr_id ={idPers}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new Medical
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1),
                    DateBegin = reader["date_start"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                    DateEnd = reader["date_finish"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                });
            }
            return list;
        }

        private static async Task<IEnumerable<HistoryBook>> GetHistoryBooksAsync(int idPerson)
        {
            var list = new List<HistoryBook>();
            var sql =
                $"select tk.n_zap , tk.date_zap , tk.info , tk.staj_ob ,tk.staj_ped, tk.staj_nauch , tk.staj_lgpu , tk.staj_bibl, tk.staj_muzei, tk.staj_med, tk.prikaz_name  from trud_knizka tk where tk.date_zap is not null and tk.sotr_id = {idPerson}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            var record = 1;
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new HistoryBook
                {
                    numberRecord = record,
                    dateInsert = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                    information = reader["info"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    isOver = reader.GetString(3) == "T",
                    isPedagogical = reader.GetString(4) == "T",
                    isScience = reader.GetString(5) == "T",
                    isUniver = reader.GetString(6) == "T",
                    isLibrary = reader.GetString(7) == "T",
                    isMuseum = reader.GetString(8) == "T",
                    isMedical = reader.GetString(9) == "T",
                    orderName = reader["prikaz_name"] != DBNull.Value ? reader.GetString(10) : "Не указано"

                });
                record++;
            }

            return list;
        }

        private static async Task<IEnumerable<Education>> GetEducationsAsync(int idPerson)
        {
            var list = new List<Education>();
            var sql =
                $"select distinct ed.uch_zav , ed.typ_obr , ed.spec , ed.kvalification , ed.date_vidachy ,ed.n_diploma , ed.is_osn from education ed where  ed.sotr_id = {idPerson}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new Education
                {
                    institution = reader["uch_zav"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    type = reader.GetString(1),
                    specialty = reader["spec"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    qualification = reader["kvalification"] != DBNull.Value ? reader.GetString(3) : "Не указано",
                    date_issue = reader["date_vidachy"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : null,
                    name_diplom = reader["n_diploma"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                    is_actual = reader.GetString(6) == "T"
                });
            }
            return list;
        }

        private static async Task<IEnumerable<Invalids>> GetInvalids(int idPerson)
        {
            var list = new List<Invalids>();
            var sql =
                $"select NUM_DOC , DATE_BEGIN , DATE_END , FOR_DEATH , GROUPE  from SOTR_INV where SOTR_INV.sotr_id = {idPerson}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                // Console.WriteLine(idPerson);
                list.Add(new Invalids
                {
                    NameDocument = reader["num_doc"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    DateBegin = reader["DATE_BEGIN"] != DBNull.Value ? reader.GetDateTime(1).ToString("yyyy-MM-dd") : "1900-01-01",
                    DateEnd = reader["DATE_END"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                    ForLife = reader.GetString(3) == "T",
                    Group = reader["GROUPE"] != DBNull.Value ? reader.GetInt16(4) : 0
                });
            }
            return list;
        }

        private static async Task<IEnumerable<Pensioners>> GetPensionerAsync(int idPerson)
        {
            var list = new List<Pensioners>();

            var sql =
                $"select num_doc , date_doc , PRICINA_VYHODA  from SOTR_PENS where SOTR_PENS.sotr_id = {idPerson}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new Pensioners
                {
                    Document = reader["num_doc"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    DateDocument = reader["date_doc"] != DBNull.Value ? reader.GetDateTime(1).ToString("yyyy-MM-dd") : "1900-01-01",
                    TypeDocument = reader["PRICINA_VYHODA"] != DBNull.Value ? reader.GetString(2).ToLower() : "вид не указан"
                });
            }
            return list;
        }

        private static async Task<IEnumerable<Family>> GetFamiliesAsync(int idPers)
        {
            var list = new List<Family>();
            var sql = "select f.typ , f.FIO , f.prim , f.DATE_BIRTH from FAMILY f where f.sotr_id = " + idPers;
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new Family
                {
                    Type = reader.GetString(0),
                    FullName = reader.GetString(1),
                    Description = reader["prim"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    Birthday = reader["DATE_BIRTH"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null

                });
            }
            return list;
        }

        private static async Task<IEnumerable<ChangeSurname>> GetChangeSurname(int idPers)
        {
            var array = new List<ChangeSurname>(); 
            var sql =
                $" select CF.ex_famil , P.name , P.date_crt , P.typ from CHANGED_FAMILS CF inner join prikaz p on p.id = CF.prikaz_id where CF.sotr_id = {idPers}";
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return array;
            while (await reader.ReadAsync())
            {
                var typeOrder =
                    reader.GetString(3) == "famil" ? "Смена фамилии" : "";

                array.Add(new ChangeSurname
                {
                    OldSurname = reader["ex_famil"] != DBNull.Value ? reader.GetString(0) : "Не указано",
                    Order = $"{reader.GetString(1)}(от {reader.GetDateTime(2).ToShortDateString()})",
                    TypeOrder = typeOrder,
                    DateOrder = reader.GetDateTime(2).ToString("yyyy-MM-dd"),
                });
            }
            return array;
        }

        private static async Task<IEnumerable<PersonPosition>> GetPersonPosition(int idPerson)
        {
            var list = new List<PersonPosition>();
            var sql =
                $"select d.name as DNAME,o.name as ONAME,pr.name as ORDERS,pr.date_crt as DATEORDER,pr.typ as TYPEORDER,td.name as CONTRACT,mj.name as PLACE,sd.is_osn as ISMAIN,sd.kolvo_b,sd.kolvo_nb,s.is_outher ,d.is_ped ,sd.date_kontr_nach,sd.date_kontr_kon,sd.dat_drop,sd.dolj_uvoln from sotr s inner join sotr_doljn sd on s.id = sd.sotr_id left join MESTO_JOB mj on mj.id = sd.MESTO_JOB left join TYP_DOGOVOR td on td.id = sd.typ_dog inner join prikaz pr on pr.id = sd.prikaz_id inner join doljnost d on d.id = sd.dolj_id inner join otdel o on o.id = d.otdel_id where s.id = {idPerson}";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                var typeOrder =
                    reader.GetString(4) == "priyom" ? "Приём" :
                    reader.GetString(4) == "uvolnenie" ? "Увольнение" :
                    reader.GetString(4) == "perev" ? "Перевод" :
                    reader.GetString(4) == "otpusk" ? "Отпуск" :
                    reader.GetString(4) == "voensbor" ? "Военные сборы" :
                    reader.GetString(4) == "prodstd" ? "Продление СТД" :
                    reader.GetString(4) == "den" ? "Денежная компенсация за неиспользованный отпуск" :
                    "";
                list.Add(new PersonPosition
                {
                    Position = reader.GetString(0).Trim(),
                    Department = reader.GetString(1).Trim(),
                    Order = $"{reader.GetString(2)}(от {reader.GetDateTime(3).ToShortDateString()})",
                    DateOrder = reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                    TypeOrder = typeOrder,
                    Contract = reader["CONTRACT"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                    Place = reader.GetString(6),
                    IsMain = reader.GetString(7) == "T",
                    CountBudget = reader.GetDecimal(8),
                    CountNoBudget = reader.GetDecimal(9),
                    IsPluralismOter = reader.GetString(10) == "T", //Внешний совместитель
                    IsPluralismInner = (reader.GetString(7) == "F" && reader.GetString(10) == "F"), // Совместитель
                    IsPed = reader.GetString(11) == "T",
                    DateStartContract = reader["date_kontr_nach"] != DBNull.Value ? reader.GetDateTime(12).ToString("yyyy-MM-dd") : null,
                    DateEndContract = reader["date_kontr_kon"] != DBNull.Value ? reader.GetDateTime(13).ToString("yyyy-MM-dd") : null,
                    DateDrop = reader["dat_drop"] != DBNull.Value ? reader.GetDateTime(14).ToString("yyyy-MM-dd") : null,
                    PositionDrop = reader["dolj_uvoln"] != DBNull.Value ? reader.GetString(15) : null,
                });

            }
            return list;
        }

        #endregion

        #region Список отпусков всех сотрудников

        public static async Task<IEnumerable<Vacations>> GetVacations( int first = 1000 , int skip = 0)
        {
            var list = new List<Vacations>();
        string sql =
                           " select " +
                           $" first {first} " +
                           $" skip {skip} " +
                           " distinct o.sotr_id , o.period , o.dlina ,o.ostatok, o.date_nach , o.date_kon,  p.name , p.date_crt , t.name as typ , o.description" +
                           " from otpusk o " +
                           " inner join typ_otpusk t on o.typ_nick = t.nick " +
                           " left join prikaz p on o.prikaz_id = p.id " +
                           " inner join sotr_doljn sd on o.SOTR_ID = sd.sotr_id " +

                           // " where sd.dolj_id <> 0  and o.typ_nick = 'lik' " +
                               " order by o.sotr_id desc";
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Vacations
                {
                    PersonId = reader.GetInt32(0),
                    Period = reader["period"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                    Length = reader["dlina"] != DBNull.Value ? reader.GetInt32(2) : 0,
                    Ostatok = reader["ostatok"] != DBNull.Value ? reader.GetInt32(3) : 0,
                    DateStart = reader["date_nach"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : null,
                    DateEnd = reader["date_kon"] != DBNull.Value ? reader.GetDateTime(5).ToString("yyyy-MM-dd") : null,
                    OrderName = reader["name"] != DBNull.Value ? $"{reader.GetString(6)}(от {reader.GetDateTime(7).ToShortDateString()})" : null,
                    DateOrder = reader["date_crt"] != DBNull.Value ?  reader.GetDateTime(7).ToString("yyyy-MM-dd") : DateTime.MinValue.ToString(),
                    TypeVacation = reader.GetString(8),
                    Description = reader["description"] != DBNull.Value ? reader.GetString(9) : null
                });

            }
            return list;
        }

        #endregion

        #region Список документов

        public static async Task<int> GetCountDocuments()
        {
            const string sql = "select" +
                               " count (distinct s.id) " +
                               " from sotr s " +
                               " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                               " inner join sotr_document sdd on s.id = sdd.sotr_id " +
                               " inner join typ_sotr_doc td on sdd.typ = td.id " +
                                " where sd.dolj_id = 0 " +
                               //" where sd.dolj_id <> 0 " +
                               "and sdd.name is not null ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            return reader.Read() ? reader.GetInt32(0) : 0;
        }
        public static async Task<IEnumerable<Documents>> GetDocumentsAsync(int first = 100 , int skip = 0)
        {
            var list = new List<Documents>();
            //string path = "D:\\documents\\";
            var sql =
                $"select first {first} skip {skip} distinct s.id, td.name,  sdd.doc , sdd.name  from sotr s   inner join sotr_doljn sd on s.id = sd.sotr_id  inner join sotr_document sdd on s.id = sdd.sotr_id  inner join typ_sotr_doc td on sdd.typ = td.id " +
                // $" where sd.dolj_id = 0 " +
                $" where sd.SOTR_ID = 5454 " +
                $" and sdd.name is not null and sdd.doc is not null  order by s.id desc ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            int row = 0;
            while (await reader.ReadAsync())
            {
                
                list.Add(new Documents
                {
                    IdPers = reader.GetInt32(0),
                    Type = reader.GetString(1),
                    Document = reader["doc"] != DBNull.Value ? reader["doc"] as byte[] : null,
                    Name = $"{reader.GetString(3)}-{row++}",

                });
                await Task.Delay(100);
            }
            return list;
        }

        #endregion

        #region Список награждений
        public static async Task<IEnumerable<Rewarding>> GetRewarding()
        {
            var list = new List<Rewarding>();
            const string sql = " select distinct s.id , tn.name , sn.prim , p.name as order_name, p.date_crt as date_order  , sn.date_crt as date_issuing " +
                               " from sotr s " +
                               " inner join sotr_nagrad sn on s.id = sn.sotr_id " +
                               " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                               " inner join typ_nagrady tn on sn.nagrad_id = tn.id " +
                               " left join prikaz p on p.id = sn.prikaz_id " +
                               //" where sd.dolj_id = 0 " +
                               "order by s.id desc";
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Rewarding
                {
                    PersonId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    NumberDocument = reader["prim"] != DBNull.Value ? reader.GetString(2) : string.Empty,
                    OrderName = reader["date_order"] != DBNull.Value ? $"{reader.GetString(3)}(от {reader.GetDateTime(4).ToShortDateString()})" : null,
                    DateOrder = reader["date_order"] != DBNull.Value ? reader.GetDateTime(4).ToString("yyyy-MM-dd") : null,
                    DateIssuing = reader["date_issuing"] != DBNull.Value ? reader.GetDateTime(5).ToString("yyyy-MM-dd") : null,
                });
            }
            return list;
        }

        #endregion

        #region Повыщение квалификации
        public static async Task<IEnumerable<Qualification>> GetQualification()
        {
            var list = new List<Qualification>();
            const string sql = $"select distinct s.id, kv.kurs_name , kv.date_nach , kv.date_kon , kv.mesto_prohogd ,kv.n_svid , kv.date_vidachy " +
                               " from sotr s " +
                               " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                               " inner join kvalification kv on kv.sotr_id = s.id " +
                               // " where sd.dolj_id <> 0 " +
                               "and kv.kurs_name is not null  order by s.id desc ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Qualification
                    {
                        IdPerson = reader.GetInt32(0),
                        CourseName = reader.GetString(1),
                        DateBegin = reader["date_nach"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                        DateEnd = reader["date_kon"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                        Place = reader["mesto_prohogd"] != DBNull.Value ? reader.GetString(4) : "Не указано",
                        NumberCertificate = reader["n_svid"] != DBNull.Value ? reader.GetString(5) : "Не указано",
                        DateIssue = reader["date_vidachy"] != DBNull.Value ? reader.GetDateTime(6).ToString("yyyy-MM-dd") : null,
                    });
                }
            }
            await reader.CloseAsync();
            return list;
        }
        #endregion

        #region Ученое Звание
        public static async Task<IEnumerable<UchZvanie>> GetUchZvanieList()
        {
            var list = new List<UchZvanie>();

        const string sql = "select distinct s.id , tp.name, uz.kafedra , uz.date_prisv_zvan , uz.n_docum, uz.place_prisv_zvan " +
                           "from sotr s " +
                           " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                           " inner join uch_zvan uz on s.id = uz.sotr_id " +
                           " inner join typ_zvan tp on uz.zvan_id = tp.id ";
                                //" where sd.dolj_id <> 0 ";
                               // " where sd.dolj_id = 0 ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                list.Add(new UchZvanie
                {
                    IdPerson = reader.GetInt32(0),
                    Type = reader.GetString(1),
                    Department = reader["kafedra"] != DBNull.Value ? reader.GetString(2) : "Не указано",
                    DateDateIssue = reader["date_prisv_zvan"] != DBNull.Value ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : null,
                    Document = reader["n_docum"] != DBNull.Value ? reader.GetString(4) : "Не указано",
                    Place = reader["place_prisv_zvan"] != DBNull.Value ? reader.GetString(5) : "Не указано"
                });
            }
            return list;

        }
        #endregion

        #region Ученая степень
        public static async Task<IEnumerable<ScientificDegree>> GetScientificDegrees()
        {
            var list = new List<ScientificDegree>();
        const string sql = $" select distinct ns.sotr_id " +
                           ", ns.n_diploma" +
                           ", ns.date_vyd" +
                           ", ns.stepen_nick" +
                           ", ns.nauk_otrasl" +
                           ", ns.nauk_spec" +
                           ", ns.tema_disert" +
                           ", ns.place_prisv_zvan" +
                           ", ns.city_prisv" +
                           ", ns.jobs_after_asp" +
                           ", ns.kol_scientific_work" +
                           ", ns.kol_zayav_intelekt" +
                           ", ns.change_text" +
                           ", ns.comment" +
                           " from sotr s " +
                           " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                           " inner join NAUCH_STEPEN ns on ns.sotr_id = s.id ";
                               // " where sd.dolj_id <> 0 ";
                               //" where sd.dolj_id = 0 ";

        await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var academic = reader.GetString(3) == "kn" ? "Кандидат наук" : "Доктор наук";
                    list.Add(new ScientificDegree
                    {
                        PersonId = reader.GetInt32(0),
                        NumberDocument = reader["n_diploma"] != DBNull.Value ? reader.GetString(1) : "Не указано",
                        DateOfIssue = reader["date_vyd"] != DBNull.Value ? reader.GetDateTime(2).ToString("yyyy-MM-dd") : null,
                        Type = academic,
                        ScientificBranch = reader["nauk_otrasl"] != DBNull.Value ? reader.GetString(4) : "Не указано",
                        ScientificSpecialty = reader["nauk_otrasl"] != DBNull.Value ? reader.GetString(5).Replace("  ", " ") : "Не указано",
                        Dissertation = reader["tema_disert"] != DBNull.Value ? reader.GetString(6) : "Не указано",
                        Place = reader["place_prisv_zvan"] != DBNull.Value ? reader.GetString(7).Replace("  ", " ") : "Не указано",
                        City = reader["city_prisv"] != DBNull.Value ? reader.GetString(8) : "Не указано",
                        JobAfter = reader["jobs_after_asp"] != DBNull.Value ? reader.GetString(9) : "Не указано",
                        CountScientific = reader["kol_scientific_work"] != DBNull.Value ? reader.GetInt32(10) : 0,
                        RequestScientific = reader["kol_zayav_intelekt"] != DBNull.Value ? reader.GetString(11) : "",
                        Change = reader["change_text"] != DBNull.Value ? reader.GetString(12) : "",
                        Description = reader["comment"] != DBNull.Value ? reader.GetString(13) : ""

                    });
                }
            }
            await reader.CloseAsync();
            return list;
        }
        #endregion

        #region Служебные перемещения

        private static async Task<bool> GetStatus(string namePosition)
        {
            try
            {
                var sql = $" select first 1 is_ped from doljnost d  where d.name = '{namePosition.Trim()}'";
                await using FbConnection connection = new(StringConnection);
                connection.Open();
                await using var transaction = await connection.BeginTransactionAsync();
                await using FbCommand command = new(sql, connection, transaction);
                await using var reader = await command.ExecuteReaderAsync();
                return await reader.ReadAsync() && reader.GetString(0) == "T";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static async Task<IEnumerable<Move>> GetMovesAsync(int first = 1000 , int skip = 0)
        {
            var list = new List<Move>();
             string sql = "select" +
                               $" first {first} " +
                               $" skip {skip} " +
                                " distinct " +
                               "sm.date_crt ," +
                               " p.name ," +
                               " p.typ ," +
                               " p.date_crt  ," +
                               " sm.kolvo_b ," +
                               " sm.kolvo_nb ," +
                               " sm.otdel_name ," +
                               " sm.dolj_name ," +
                               " sm.is_osn ," +
                               " sm.typ_dog ," +
                               " sm.date_kontr_nach ," +
                               " sm.date_kontr_kon ," +
                               " sm.date_drop ," +
                               " sm.OTP_DAYS ," +
                               " sm.prikaz_end, " +
                               " sotr_move.sotr_id," +
                               " p.DATE_CRT as dcrt "
                               + " from sotr s "
                               + " inner join sotr_doljn sd on s.id = sd.sotr_id " +
                               " inner join  sotr_move sm on sm.sotr_id = s.id " +
                               " inner join prikaz p on p.id = sm.prikaz_id " +
                               " where sm.date_crt is not null and p.date_crt is not null " +
                               //" and sd.dolj_id <> 0 " +
                               "  and sm.id <> 1192 and sm.id <> 2321 and sm.id <> 565 and sm.sotr_id <> 1459 and sm.sotr_id <> 1839 " +
                               "  order by sm.date_crt desc ";
            //and sm.id <> 1192 and sm.id <> 2321 and sm.id <> 565 and sm.sotr_id <> 1459 and sm.sotr_id <> 1839
            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) return list;
            while (await reader.ReadAsync())
            {
                var typeOrder =
                    reader.GetString(2) == "priyom" ? "Приём" :
                    reader.GetString(2) == "uvolnenie" ? "Увольнение" :
                    reader.GetString(2) == "perev" ? "Перевод" :
                    reader.GetString(2) == "prodstd" ? "Продление СТД" :
                    reader.GetString(2) == "den" ? "Денежная компенсация за неиспользованный отпуск" :
                    "";

                var contract = reader["typ_dog"] != DBNull.Value ? reader.GetString(9) : "Не указано";
                var dateCrt = reader["dcrt"] != DBNull.Value ? reader.GetDateTime(3).ToShortDateString() : "";

                var position = reader["dolj_name"] != DBNull.Value ? reader.GetString(7) : "Не указано";

                if (reader.GetString(9) == "Постійне місце роботи")
                {
                    Console.WriteLine(reader.GetInt32(15));
                    contract = "Постоянное место";
                }
                if (reader.GetString(9) == "Лікарняний лист")
                {
                    contract = "Больничный лист";
                }


                list.Add(new Move
                {
                    dateInsert = reader.GetDateTime(0).ToString("yyyy-MM-dd"),
                    order = $"{reader.GetString(1)} от({dateCrt})",
                    typeOrder = typeOrder,
                    count_budget = reader["kolvo_b"] != DBNull.Value ? reader.GetDecimal(4) : 0,
                    count_nobudget = reader["kolvo_nb"] != DBNull.Value ? reader.GetDecimal(5) : 0,
                    name_dep = reader["otdel_name"] != DBNull.Value ? reader.GetString(6) : "Не указано",
                    position = position,
                    isMain = reader.GetString(8) == "T",
                    Contract = reader["typ_dog"] != DBNull.Value ? reader.GetString(9) : "Не указано",
                    dateBegin = reader["date_kontr_nach"] != DBNull.Value ? reader.GetDateTime(10).ToString("yyyy-MM-dd") : null,
                    dateEnd = reader["date_kontr_kon"] != DBNull.Value ? reader.GetDateTime(11).ToString("yyyy-MM-dd") : null,
                    dateDrop = reader["date_drop"] != DBNull.Value ? reader.GetDateTime(12).ToString("yyyy-MM-dd") : null,
                    dayVacation = reader["OTP_DAYS"] != DBNull.Value ? reader.GetInt32(13) : 0,
                    orderDrop = reader["prikaz_end"] != DBNull.Value ? reader.GetString(14).Replace("  ", " ") : null,
                    PersonId = reader.GetInt32(15),
                    DateOrder = reader["dcrt"] != DBNull.Value ? reader.GetDateTime(16).ToString("yyyy-MM-dd") : null,
                    isPed =  await GetStatus(position),

                });
            }
            //var t = list.Where(x => Convert.ToDateTime(x.dateInsert) > new DateTime(2014, 1, 1));
            return list;

        }
        #endregion

        #region Генерация фотографий 3x4
        public static async Task<IEnumerable<Image>> GetPhoto(int first = 100, int skip = 0)
        {
            var list = new List<Image>();
            var sql =
                $"select first {first} skip {skip} s.id, s.photo from sotr s inner join sotr_doljn sd on s.id = sd.sotr_id  where s.photo is not null " +
                //$" and sd.dolj_id = 0 "
                $"order by s.id desc ";

            await using FbConnection connection = new(StringConnection);
            connection.Open();
            await using var transaction = await connection.BeginTransactionAsync();
            await using FbCommand command = new(sql, connection, transaction);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                try
                {
                    list.Add(new Image
                    {
                        id_person = reader.GetInt32(0),
                        photo = reader["photo"] is byte[] ? Convert.ToBase64String(reader["photo"] as byte[] ?? Array.Empty<byte>()) : null
                        //photo = (byte[])reader["photo"],
                    });

                }
                catch (Exception)
                {
                    Console.WriteLine(reader.GetInt32(0));
                }
               
            }
            return list;

        }
        #endregion

}