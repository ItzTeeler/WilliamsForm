using BackendRedo.Models;
using BackendRedo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendRedo.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly FormService _data;
        public FormController(FormService data)
        {
            _data = data;
        }

        [HttpPost]
        [Route("AddForm")]
        public bool AddForm (FormModel form){
            return _data.AddForm(form);
        }

        [HttpGet]
        [Route("GetForm")]
        public IEnumerable<FormModel> GetForm (){
            return _data.GetForm();
        }
    }

