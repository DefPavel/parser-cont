namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SyncEducationController : ControllerBase
{
    private const string Hosting = "http://localhost:8080";

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayStudents>> AllSpecialty(string token)
    {
        using ClientApi client = new(Hosting);
        ArrayStudents globalArray = new()
        {
            ArraySpecialty = await FirebirdService.GetNewSpecialty()
        };
        // Если запрос пустой
        return globalArray.ArraySpecialty.Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/specialty", token, globalArray);
    }

    [Route("")]
    [HttpPost]

    public async Task<ActionResult<ArrayStudents>> StudentByIdDepartment(string token, string idFaculty)
    {
        using ClientApi client = new(Hosting);
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudents(idFaculty)
        };
        // Сформировать json файл
        /*await using var createStream = System.IO.File.Create(@"students.json");
        // Сериализация в UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        JsonSerializerOptions options = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        
        await JsonSerializer.SerializeAsync(createStream, globalArray, options);
        */

        // Если запрос пустой
        return globalArray.ArrayStudent.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/students", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayMarks>> StudentMarksByGroups(string token , int idCont)
    {
        using ClientApi client = new(Hosting);

        ArrayMarks globalArray = new();
        List<NewGroups> newGroups = new();
        foreach (var item in newGroups)
        {
            item.Id = idCont;
            item.IdCont = idCont;
            item.ArrayStudents = await FirebirdService.GetStudentMarks(item.IdCont);
            // globalArray.Arrays = await FirebirdService.GetStudentMarks(item.IdCont);

            // itemGroups.Add(item);
            globalArray.Arrays.Add(item);
        }
        return newGroups.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayMarks>(@"/api/sync/cont/marksToGroup", token, globalArray);
    }
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayMarks>> StudentMarksByFaculty(string token , int idDepartment , int idForm , int idLevel , int course)
    {
        using ClientApi client = new(Hosting);

    // Get Groups for JMU
        var groups = await client.GetAsyncByToken<List<NewGroups>>($"/api/education/groups/tree/all?idDepartment={idDepartment}&idForm={idForm}&idLevel={idLevel}&course={course}", token);
        ArrayMarks globalArray = new();
        // Потом убери first items
        foreach (var item in groups)
        {

            item.ArrayStudents = await FirebirdService.GetStudentMarks(item.IdCont);
            // globalArray.Arrays = await FirebirdService.GetStudentMarks(item.IdCont);
           
            // itemGroups.Add(item);
            globalArray.Arrays.Add(item);
        }
        return await client.PostAsyncByToken<ArrayMarks>(@"/api/sync/cont/marksToGroup", token, globalArray);
    }

    /*[Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayStudents>> StudentByAllDepartment(string token)
    {
        using ClientApi client = new(Hosting);
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudentsAll()
        };
        /*await using var createStream = System.IO.File.Create(@"students.json");

        // Сериализация в UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        await JsonSerializer.SerializeAsync(createStream, globalArray, options);
        // Если запрос пустой
        return globalArray.ArrayStudent.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/students", token, globalArray);
    }
*/
   
}