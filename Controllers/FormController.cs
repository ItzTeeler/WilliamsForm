using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendRedo.Models;
using BackendRedo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendRedo.Controllers
{
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
        public bool AddForm(FormModel NewForm)
        {
            return _data.AddForm(NewForm);
        }

        [HttpGet]
        [Route("GetAllForms")]
        public IEnumerable<FormModel> GetAllForms (){
            return _data.GetAllForms();
        }

        [HttpPut]
        [Route("UpdateForm")]
        public bool UpdateForm(FormModel formToUpdate)
        {
            return _data.UpdateForm(formToUpdate);
        }

        [HttpGet]
        [Route("GetFormById")]
        public FormModel GetFormById(int id)
        {
            return _data.GetFormById(id);
        }

        [HttpDelete]
        [Route("DeleteFormById/{formToDelete}")]
        public bool DeleteFormById(int formToDelete)
        {
            return _data.DeleteFormById(formToDelete);
        }
    }


}