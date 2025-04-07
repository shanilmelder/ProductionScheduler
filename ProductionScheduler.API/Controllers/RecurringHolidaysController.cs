using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Models;
using ProductionScheduler.Models.Request;
using ProductionScheduler.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductionScheduler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringHolidaysController : ControllerBase
    {
        private readonly IRecurringHolidayRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecurringHolidaysController(IRecurringHolidayRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<RecurringHolidaysController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var holidays = await _repository.GetAsync();
            return Ok(holidays);
        }

        // GET api/<RecurringHolidaysController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var holiday = await _repository.GetByIdAsync(id);
            return Ok(holiday);
        }

        // POST api/<RecurringHolidaysController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecurringHolidayRequest holidayRequest)
        {
            var holiday = _mapper.Map<RecurringHoliday>(holidayRequest);
            await _repository.AddAsync(holiday);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<RecurringHolidaysController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RecurringHolidayRequest holidayRequest)
        {
            var holiday = _mapper.Map<RecurringHoliday>(holidayRequest);
            _repository.Update(holiday);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<RecurringHolidaysController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
