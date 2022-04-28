namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SyncPersonnelController : ControllerBase
{
    private const string Hosting = "http://localhost:8080";
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
        GlobalArray globalArray = new()
        {
            ArrayDepartments = await FirebirdServicePersonnel.GetDepartment()
        };
        return globalArray.ArrayDepartments.Count == 0
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

        GlobalArray globalArray = new()
        {
            ArrayPersons = await FirebirdServicePersonnel.GetPersonsAsync()
        };
        return globalArray.ArrayPersons.Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync", token, globalArray);
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
        
        GlobalArray globalArray = new()
        {
            ArrayVacation = await FirebirdServicePersonnel.GetVacations()
        };
        return globalArray.ArrayVacation.Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/vacation/sync", token, globalArray);
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
        return globalArray.ArrayRewarding.Count == 0
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

        return globalArray.ArrayQualification.Count == 0
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
        return globalArray.ArrayAcademicTitle.Count == 0
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
        return globalArray.ArrayDegrees.Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/scientific/sync", token, globalArray);
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<GlobalArray>> Moves(string token)
    {
        using ClientApi client = new(Hosting);
        GlobalArray globalArray = new()
        {
            ArrayMove = await FirebirdServicePersonnel.GetMovesAsync()
        };

        return globalArray.ArrayMove.Count == 0
            ? new BadRequestResult()
            : await client.PostAsyncByToken<GlobalArray>(@"api/pers/relocation/sync", token, globalArray);
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
            if (globalArray.ArrayImage.Count > 0)
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
            globalArray.ArrayDocuments = await FirebirdServicePersonnel.GetDocumentsAsync(25, skip);

            if (globalArray.ArrayDocuments.Count > 0)
            {
                await client.PostAsyncByToken<GlobalArray>(@"api/pers/document/sync", token, globalArray);
                skip += 100;
            }
            else
            {
                return new OkResult();
            }
        }
        return new OkResult();
    }

}