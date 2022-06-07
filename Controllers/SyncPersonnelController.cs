using System.Diagnostics;

namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SyncPersonnelController : ControllerBase
{

    private readonly ILogger _logger;
    private const string Hosting = "http://localhost:8080";
    public SyncPersonnelController(ILogger<SyncPersonnelController> logger) 
    {
        _logger = logger; 
    }
    /// <summary>
    /// Get Departments and Positions
    /// </summary>
    /// <param name="token">JWT TOKEN</param>
    /// <returns>ArrayDepartments</returns>
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> DepartmentAndPosition(string token)
    {
        using ClientApi client = new(Hosting);
        Stopwatch stopWatch = new();
        stopWatch.Start();
        GlobalArray globalArray = new()
        {
            ArrayDepartments = await FirebirdServicePersonnel.GetDepartment()
        };
        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        _logger.LogInformation(message: $"Затраченно времени на коллекцию Отделов и должностей: (Часов:{ts.Hours};Минут:{ts.Minutes};Секунд:{ts.Seconds};)");
        _ = globalArray.ArrayDepartments.ToList();
        return globalArray.ArrayDepartments.ToList().Count == 0
           ? new BadRequestResult()
           : await client.PostAsyncByToken<GlobalArray>(@"api/pers/tree/sync", token, globalArray);
    }

    /// <summary>
    /// Get Persons
    /// </summary>
    /// <param name="token">JWT TOKEN</param>
    /// <returns>ArrayPersons</returns>
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Persons(string token)
    {
        using ClientApi client = new(Hosting);

        var countPerson = await FirebirdServicePersonnel.GetCountAllPersons();
        var skip = 0;
        GlobalArray globalArray = new();
        //Stopwatch stopWatch = new();
        for (var i = 0; i < countPerson; i++)
        {
            globalArray.ArrayPersons = await FirebirdServicePersonnel.GetPersonsAsync(500, skip);
            if (globalArray.ArrayPersons.ToList().Count > 0)
            {
                //stopWatch.Start();
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync", token, globalArray);
                skip += 500;
               // stopWatch.Stop();
                //var ts = stopWatch.Elapsed;
                _logger.LogInformation(message: $"Отправлено {skip}");
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();

        /*Stopwatch stopWatch = new();
        stopWatch.Start();
        GlobalArray globalArray = new()
        {
            ArrayPersons = await FirebirdServicePersonnel.GetPersonsAsync()
        };
        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        _logger.LogInformation(message: $"Затраченно времени на коллекцию Сотрудников: (Часов:{ts.Hours};Минут:{ts.Minutes};Секунд:{ts.Seconds};)");
        return globalArray.ArrayPersons.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync", token, globalArray);
        */
    }

    /// <summary>
    /// Get Vacations of Person
    /// </summary>
    /// <param name="token">JWT TOKEN</param>
    /// <returns>ArrayVacation</returns>
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Vacations(string token)
    {
        using ClientApi client = new(Hosting);
        var skip = 0;
        GlobalArray globalArray = new();

        for (var i = 0; i < 50; i++)
        {
            globalArray.ArrayVacation = await FirebirdServicePersonnel.GetVacations(2000, skip);
            if (globalArray.ArrayVacation.ToList().Count > 0)
            {
                await Task.Delay(500);
                //stopWatch.Start();
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/vacation/sync", token, globalArray);
                skip += 2000;
                // stopWatch.Stop();
                //var ts = stopWatch.Elapsed;
                _logger.LogInformation(message: $"Отправлено {skip}");
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();

    }

    /// <summary>
    /// Get Rewarding of Person
    /// </summary>
    /// <param name="token">JWT TOKEN</param>
    /// <returns>ArrayRewarding</returns> 
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Rewarding(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayRewarding = await FirebirdServicePersonnel.GetRewarding()
        };
        return globalArray.ArrayRewarding.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/rewarding/sync", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Qualifications(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayQualification = await FirebirdServicePersonnel.GetQualification()
        };

        return globalArray.ArrayQualification.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/qualification/sync", token, globalArray);
      
    }
    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> AcademicTitles(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayAcademicTitle = await FirebirdServicePersonnel.GetUchZvanieList()
        };
        return globalArray.ArrayAcademicTitle.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/academicTitle/sync", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> ScientificDegrees(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayDegrees = await FirebirdServicePersonnel.GetScientificDegrees()
        };
        return globalArray.ArrayDegrees.ToList().Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/scientific/sync", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Moves(string token)
    {
        var skip = 0;
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayMove = await FirebirdServicePersonnel.GetMovesAsync()
        };

        for (var i = 0; i < 20; i++)
        {
            globalArray.ArrayMove = await FirebirdServicePersonnel.GetMovesAsync(2000, skip);
            if (globalArray.ArrayMove.ToList().Count > 0)
            {
                await Task.Delay(500);
                //stopWatch.Start();
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/relocation/sync", token, globalArray);
                skip += 2000;
                // stopWatch.Stop();
                //var ts = stopWatch.Elapsed;
                _logger.LogInformation(message: $"Отправлено {skip}");
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Avatars(string token)
    {
        using ClientApi client = new(Hosting);
        var countPerson = await FirebirdServicePersonnel.GetCountPersons();
        var skip = 0;
        GlobalArray globalArray = new();
        for (var i = 0; i < countPerson; i++)
        {
            globalArray.ArrayImage = await FirebirdServicePersonnel.GetPhoto(100, skip);
            if (globalArray.ArrayImage.ToList().Count > 0)
            {
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync/image", token, globalArray);
                skip += 100;
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Documents(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new();
        var countDocuments = await FirebirdServicePersonnel.GetCountDocuments();
        var skip = 0;
        
        for (var i = 0; i < countDocuments; i++)
        {
            globalArray.ArrayDocuments = await FirebirdServicePersonnel.GetDocumentsAsync(10, skip);

            if (globalArray.ArrayDocuments.ToList().Count > 0)
            {
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/document/sync", token, globalArray);
                skip += 10;
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();
    }

}