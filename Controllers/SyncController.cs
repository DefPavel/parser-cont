using System.Text.Encodings.Web;

namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]")]
public class SyncController : ControllerBase
{
    private const string Hosting = "http://localhost:8080";

    [Route("/[action]")]
    [HttpPost]
    public async Task<ActionResult<ArrayStudents>> StudentByIdDepartment(string token , string idFaculty)
    {
        using ClientApi client = new(Hosting);
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudents(idFaculty)
        };
        // Записать в файл json
        /*using FileStream createStream = System.IO.File.Create(@"students.json");

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

    [Route("/[action]")]
    [HttpPost]
    public async Task<ActionResult<ArrayStudents>> StudentByAllDepartment(string token)
    {
        using ClientApi client = new(Hosting);
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudentsAll()
        };
        // Если запрос пустой
        return globalArray.ArrayStudent.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/students", token, globalArray);
    }
    [Route("/[action]")]
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

