using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecordMyStats.Common;
using RecordMyStats.Common.Dto;
using RecordMyStats.Common.Entities;
using RecordMyStats.DataAccess.Data.Vitals;
using RecordMyStats.WebApi2.Services;

namespace RecordMyStats.WebApi2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntryController : ControllerBase
    {
        private IVitalsRepository repos = VitalsRepositoryFactory.GetVitalsRepository();

        private readonly ILogger<EntryController> _logger;

        IConfiguration _configuration;

        public EntryController(ILogger<EntryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("GetEntriesBySessionKey")]
        public ActionResult<GetEntriesResultDto> GetEntriesBySessionKey(string sessionKey)
        {
            if (sessionKey == null)
            {
                var missingParamsResult = new GetEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetEntriesBySessionKey(sessionKey, out string errors);

            var normalResult = new GetEntriesResultDto()
            {
                Entries = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };

            return Ok(normalResult);
        }

        [Authorize]
        [HttpPost("GetQuestionsBySessionKey")]
        public ActionResult<GetQuestionsResultDto> GetQuestionsBySessionKey(string sessionKey)
        {
            if (sessionKey == null)
            {
                var missingParamsResult = new GetQuestionsResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetQuestionsBySessionKey(sessionKey, true, out string errors);

            var normalResult = new GetQuestionsResultDto()
            {
                Questions = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };

            return Ok(normalResult);
        }

        [Authorize]
        [HttpPost("GetBloodSugarEntriesBySessionKey")]
        public ActionResult<GetBloodSugarEntriesResultDto> GetBloodSugarEntriesBySessionKey(string sessionKey)
        {
            if (sessionKey == null)
            {
                var missingParamsResult = new GetEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetBloodSugarEntriesBySessionKey(sessionKey, out string errors);

            var normalResult = new GetBloodSugarEntriesResultDto()
            {
                Entries = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };

            return Ok(normalResult);
        }

        [Authorize]
        [HttpPost("GetBloodPressureEntriesBySessionKey")]
        public ActionResult<GetBloodSugarEntriesResultDto> GetBloodPressureEntriesBySessionKey(string sessionKey)
        {
            if (sessionKey == null)
            {
                var missingParamsResult = new GetEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetBloodPressureEntriesBySessionKey(sessionKey, out string errors);

            var normalResult = new GetBloodPressureEntriesResultDto()
            {
                Entries = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };

            return Ok(normalResult);
        }

        [Authorize]
        [HttpPost("GetBloodSugarEntriesByRange")]
        public ActionResult<GetBloodSugarEntriesResultDto> GetBloodSugarEntriesByRange(GetEntriesParamsDto rangeParams)
        {
            if (rangeParams.SessionKey == null)
            {
                var missingParamsResult = new GetEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetBloodSugarEntriesBySessionKey(rangeParams.SessionKey, rangeParams.DateFrom, rangeParams.DateTo, out string errors);

            var successResult = new GetBloodSugarEntriesResultDto()
            {
                Entries = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("GetEntriesByRange")]
        public ActionResult<GetBloodSugarEntriesResultDto> GetEntriesByRange(GetEntriesParamsDto rangeParams)
        {
            if (rangeParams.SessionKey == null)
            {
                var missingParamsResult = new GetEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetEntriesBySessionKey(rangeParams.SessionKey, rangeParams.DateFrom, rangeParams.DateTo, out string errors);

            var successResult = new GetEntriesResultDto()
            {
                Entries = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("GetNoteEntriesByRange")]
        public ActionResult<GetNoteEntriesResultDto> GetNoteEntriesByRange(GetNoteEntriesParamsDto rangeParams)
        {
            if (rangeParams.SessionKey == null)
            {
                var missingParamsResult = new GetNoteEntriesResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }
            var result = repos.GetNoteEntriesBySessionKey(rangeParams.SessionKey, rangeParams.DateFrom, rangeParams.DateTo, out string errors);

            var successResult = new GetNoteEntriesResultDto()
            {
                Notes = result,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors)
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("AddEntry")]
        public ActionResult<SimpleResultDto> AddEntry(AddEntryDto addEntryDto)
        {
            if (addEntryDto == null || addEntryDto.StatisticEntry == null || addEntryDto.SessionKey == null)
            {
                var missingParamsResult = new SimpleResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }

            bool result = repos.AddEntry(addEntryDto.StatisticEntry, addEntryDto.SessionKey, out string errors);

            // test call, wip stuff, verifying we can get info from the raw Token if required
            var email1 = TokenUtility.GetEmailFromToken(Request, _configuration);

            var successResult = new SimpleResultDto()
            {
                Errors = errors,
                Result = result
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("AddNoteEntry")]
        public ActionResult<SimpleResultDto> AddNoteEntry(AddNoteEntryDto addNoteEntryDto)
        {
            if (addNoteEntryDto == null || addNoteEntryDto.NoteEntry == null || addNoteEntryDto.SessionKey == null)
            {
                var missingParamsResult = new SimpleResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }

            bool result = repos.AddNoteEntry(addNoteEntryDto.NoteEntry, addNoteEntryDto.SessionKey, out string errors);

            // test call, wip stuff, verifying we can get info from the raw Token if required
            var email1 = TokenUtility.GetEmailFromToken(Request, _configuration);

            var successResult = new SimpleResultDto()
            {
                Errors = errors,
                Result = result
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("AddBloodSugarEntry")]
        public ActionResult<SimpleResultDto> AddBloodSugarEntry(AddBloodSugarEntryDto addEntryDto)
        {
            if (addEntryDto == null || addEntryDto.BloodSugarEntry == null || addEntryDto.SessionKey == null)
            {
                var missingParamsResult = new SimpleResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }

            bool result = repos.AddBloodSugarEntry(addEntryDto.BloodSugarEntry, addEntryDto.SessionKey, out string errors);

            // test call, wip stuff, verifying we can get info from the raw Token if required
            var email1 = TokenUtility.GetEmailFromToken(Request, _configuration);

            var successResult = new SimpleResultDto()
            {
                Errors = errors,
                Result = result
            };
            return Ok(successResult);
        }

        [Authorize]
        [HttpPost("AddBloodPressureEntry")]
        public ActionResult<SimpleResultDto> AddBloodPressureEntry(AddBloodPressureEntryDto addEntryDto)
        {
            if (addEntryDto == null || addEntryDto.BloodPressureEntry == null || addEntryDto.SessionKey == null)
            {
                var missingParamsResult = new SimpleResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(missingParamsResult);
            }

            bool result = repos.AddBloodPressureEntry(addEntryDto.BloodPressureEntry, addEntryDto.SessionKey, out string errors);

            // test call, wip stuff, verifying we can get info from the raw Token if required
            var email1 = TokenUtility.GetEmailFromToken(Request, _configuration);

            var successResult = new SimpleResultDto()
            {
                Errors = errors,
                Result = result
            };
            return Ok(successResult);
        }
    }
}