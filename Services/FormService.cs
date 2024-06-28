using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendRedo.Models;
using BackendRedo.Services.Context;

namespace BackendRedo.Services
{
    public class FormService
    {
        private readonly DataContext _context;
        public FormService(DataContext context)
        {
            _context = context;
        }

        public bool AddForm(FormModel NewForm)
        {
            _context.Add(NewForm);
            return _context.SaveChanges() != 0;
        }

        public IEnumerable<FormModel> GetAllForms()
        {
            return _context.FormInfo.Where(form => form.isDeleted == false);
        }

        public bool UpdateForm(FormModel formToUpdate)
        {
            _context.Update<FormModel>(formToUpdate);

            return _context.SaveChanges() != 0;
        }

        public FormModel GetFormById(int id)
        {
            return _context.FormInfo.SingleOrDefault(form => form.ID == id);
        }

        public bool DeleteFormById(int formToDelete)
        {
            FormModel foundForm = GetFormById(formToDelete);

            bool result = false;

            if (foundForm != null)
            {
                foundForm.isDeleted = true;
                _context.Update<FormModel>(foundForm);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
    }

}