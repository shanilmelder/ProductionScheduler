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
    public class OneTimeHolidaysController : ControllerBase
    {
        private readonly IOneTimeHolidayRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OneTimeHolidaysController(IOneTimeHolidayRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<OneTimeHolidaysController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var holidays = await _repository.GetAsync();
            return Ok(holidays);
        }

        // GET api/<OneTimeHolidaysController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var holiday = await _repository.GetByIdAsync(id);
            return Ok(holiday);
        }

        // POST api/<OneTimeHolidaysController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OneTimeHolidayRequest holidayRequest)
        {
            var holiday = _mapper.Map<OneTimeHoliday>(holidayRequest);
            await _repository.AddAsync(holiday);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<OneTimeHolidaysController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OneTimeHolidayRequest holidayRequest)
        {
            var holiday = _mapper.Map<OneTimeHoliday>(holidayRequest);
            _repository.Update(holiday);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<OneTimeHolidaysController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
