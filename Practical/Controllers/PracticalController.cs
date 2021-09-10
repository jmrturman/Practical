using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practical.Models;
using Practical.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class PracticalController : ControllerBase
    {
        

        private readonly IPracticalService _practicalService;
        private readonly IMapper _mapper;
        private readonly IPracticalValidationService _practicalValidationService;

        private readonly ILogger<PracticalController> _logger;
   

        public PracticalController(ILogger<PracticalController> logger, IPracticalService practicalService, IMapper mapper, IPracticalValidationService practicalValidationService)
        {
            _logger = logger;
            _practicalService = practicalService ?? throw new ArgumentNullException(nameof(practicalService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _practicalValidationService = practicalValidationService ?? throw new ArgumentNullException(nameof(practicalValidationService));
        }
        [HttpGet("{ipAddress}")]
        public async Task<IActionResult> Index(string ipAddress)
        {
            DefaultServiceList defaults = new DefaultServiceList();
            bool isValidAddress;
            var addressValidation = _practicalValidationService.Validate(ipAddress, out isValidAddress);
            if (!isValidAddress)
            {
                return Ok(addressValidation);
            }
            return Ok(await _practicalService.CallServices(ipAddress, defaults.DefaultList));
        }

        [HttpGet("{ipAddress}, {services}")]
        public async Task<IActionResult> Index(string ipAddress, string services)
        {
            var servicesList = services.Split(new char[] { ',' }).ToList();
            var addressValidation = _practicalValidationService.Validate(ipAddress, out bool isValidAddress);
            var serviceListValidation = _practicalValidationService.Validate(servicesList, out bool isValidServiceList);
            if (!isValidAddress)
            {
                return Ok(addressValidation);
            }
            if (!isValidServiceList)
            {
                return Ok(serviceListValidation);
            }
            return Ok(await _practicalService.CallServices(ipAddress, servicesList));
        }
    }
}
