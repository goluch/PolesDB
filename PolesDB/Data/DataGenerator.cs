using DataBase.Data;
using Microsoft.EntityFrameworkCore;

namespace PolesDB.Data
{
    public class DataGenerator
    {
        private readonly AppDbContext _context;
        public DataGenerator(AppDbContext context)
        {
            _context = context;
            _context.RemoveRange(_context.Persons);
            _context.RemoveRange(_context.Companies);
            _context.RemoveRange(_context.Contracts);
            _context.SaveChanges();
        }

        private void GenerateFakeCompanies()
        {
            throw new NotImplementedException();
        }

        private void GenerateFakePersons()
        {
            throw new NotImplementedException();
        }
        private void GenerateFakeContracts()
        {
            throw new NotImplementedException();
        }

        public void GenerateFakeData()
        {
            GenerateFakePersons();
            GenerateFakeCompanies();
            GenerateFakeContracts();
        }
    }
}
