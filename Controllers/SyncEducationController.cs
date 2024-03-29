﻿using System.Diagnostics;
using System.Text.Encodings.Web;

namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SyncEducationController : ControllerBase
{
    //http://jmu.api.lgpu.org
    private const string Hosting = "http://jmu.api.lgpu.org";
    private readonly ILogger _logger;
    public SyncEducationController(ILogger<SyncEducationController> logger)
    {
        _logger = logger;
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
        return globalArray.ArraySpecialty.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/specialty", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ArrayStudents> StudentByIdGroup(string idGroup)
    {
        // using ClientApi client = new(Hosting);
        Stopwatch stopWatch = new();
        stopWatch.Start();
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudentByGroup(idGroup)
        }; 
        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        _logger.LogInformation(message: $"(Затраченно времени на коллекцию Студентов idGroup={idGroup}) : (Часов:{ts.Hours};Минут:{ts.Minutes};Секунд:{ts.Seconds};)");
        return globalArray;
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayStudents>> StudentByIdDepartment(string token, string idFaculty)
    {
        using ClientApi client = new(Hosting);
        Stopwatch stopWatch = new();
        stopWatch.Start();
        ArrayStudents globalArray = new()
        {
            ArrayStudent = await FirebirdService.GetStudents(idFaculty)
        };
        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        _logger.LogInformation(message: $"(Затраченно времени на коллекцию Студентов idFaculty={idFaculty}) : (Часов:{ts.Hours};Минут:{ts.Minutes};Секунд:{ts.Seconds};)");
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
        return globalArray.ArrayStudent.ToList().Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayStudents>(@"/api/sync/cont/students", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ArrayMarks>> StudentMarksByGroups(string token , int idCont)
    {
        using ClientApi client = new(Hosting);

        ArrayMarks globalArray = new();
        var newGroups = new List<NewGroups>()
        {
            new()
            {
                Id = idCont,
                IdCont = idCont,
                ArrayStudents = await FirebirdService.GetStudentMarks(idCont),

            }
        };
        foreach (var item in newGroups)
        {
            globalArray.Arrays.Add(item);
        }
        
        /*await using var createStream = System.IO.File.Create(@"marks.json");
        // Сериализация в UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        JsonSerializerOptions options = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        
        await JsonSerializer.SerializeAsync(createStream, globalArray, options);
        */
        return newGroups.Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<ArrayMarks>(@"/api/sync/cont/marksToGroup", token, globalArray);
    }
    
    [Route("")]
    [HttpPost]
    public async Task<NewArrayMarks> MarksByStudent(int idStudentGroup)
    {
        var arrayMarks = await FirebirdService.GetNewMarks(idStudentGroup);
        var newMarksEnumerable = arrayMarks as NewMarks[] ?? arrayMarks.ToArray();
        
        NewArrayMarks globalArray = new()
        {
            IdStudent = newMarksEnumerable.Any() ? newMarksEnumerable.FirstOrDefault()!.IdStudent : 0,
            ArrayMarks = newMarksEnumerable
        };
        return globalArray;
    }
    
    [Route("")]
    [HttpPost]
    public async Task<ActionResult> NewMarksByStudentsGroup(int idGroup)
    {
        if (!Directory.Exists("jsonFiles"))
        {
            Directory.CreateDirectory("jsonFiles");
        }
        var arrayMarks = await FirebirdService.GetNewMarksGroup(idGroup);
        var newMarksEnumerable = arrayMarks as NewMarks[] ?? arrayMarks.ToArray();
        
        foreach (var item in newMarksEnumerable)
        {
            var directory = $"jsonFiles\\{item.NickFacult}_{item.NickLevel}_{item.Course}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            await using var createStream =
                System.IO.File.Create(directory + $"\\{item.FirstName} {item.MiddleName} {item.LastName}.json");

            var orderByMarksByStudent = newMarksEnumerable.Where(x => x.IdStudent == item.IdStudent);
            
            NewArrayMarks globalArray = new()
            {
                IdStudent = item.IdStudent,
                ArrayMarks = orderByMarksByStudent
            };

            // Сериализация в UTF-8
            Console.OutputEncoding = Encoding.UTF8;
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync(createStream, globalArray, options);    
        }
        
        return Ok();
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