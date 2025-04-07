using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Models;
using ProductionScheduler.Models.Entities;
using ProductionScheduler.Models.Enum;
using ProductionScheduler.Models.Request;
using ProductionScheduler.Services.Interfaces;

namespace ProductionScheduler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkDaysController : ControllerBase
    {
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IMapper _mapper;
        public WorkDaysController(IWorkDayRepository workDayRepository, IMapper mapper)
        {
            _workDayRepository = workDayRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] WorkDayRequest workDayRequest)
        {
            var workDay = _mapper.Map<WorkDay>(workDayRequest);

            if (workDay.CalculationType == CalculationType.Addition)
            {
                var completionDateTime = await _workDayRepository.AddWorkingTime(workDay);
                return Ok(completionDateTime);
            }
            else if (workDay.CalculationType == CalculationType.Subtraction)
            {
                var completionDateTime = await _workDayRepository.SubtractWorkingTime(workDay);
                return Ok(completionDateTime);
            }
            return BadRequest();
        }
    }
}
