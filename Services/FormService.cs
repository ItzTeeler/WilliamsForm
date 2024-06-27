using BackendRedo.Models;
using BackendRedo.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace BackendRedo.Services;

    public class FormService
    {
        private readonly DataContext _context;


        public FormService(DataContext context)
        {
            _context = context;
        }

        public bool AddForm (FormModel form){
          
            _context.Add(form);
            return _context.SaveChanges() != 0;
        }

        public IEnumerable<FormModel> GetForm (){
            return _context.FormInfo;
        }
    }
