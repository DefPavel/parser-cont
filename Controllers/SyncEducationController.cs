namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SyncEducationController : ControllerBase
{
     private const string Hosting = "http://localhost:8080";

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
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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
    public async Task<ActionResult<List<NewGroups>>> StudentMarksByGroups(string token)
    {
        using ClientApi client = new(Hosting);

        // Get Groups for JMU
        var groups = await client.GetAsyncByToken<List<NewGroups>>(@"/api/sync/cont/getGroups", token);

        foreach (var item in groups)
        {
            item.ArrayStudents = await FirebirdService.GetStudentMarks(item.IdCont);
        }
        return groups.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<List<NewGroups>>(@"/api/sync/cont/marksToGroup", token, groups);
    }

    [Route("")]
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
        */
        // Если запрос пустой
        return globalArray.ArrayStudent.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/students", token, globalArray);
    }
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
}