using DataBase.Data;
using DataBase.Model;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PolesDB.Data
{
    public class DataGenerator
    {
        private readonly AppDbContext _context;

        public DataGenerator(AppDbContext context)
        {
            _context = context;
            _context.Employments.ExecuteDelete();
            _context.Persons.ExecuteDelete();
            _context.Companies.ExecuteDelete();
            _context.SaveChanges();
        }
        private IList<Person> GenerateFakePersons(int count)
        {
            return new Bogus.Faker<Person>()
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.Forename, f => f.Person.FirstName)
                .RuleFor(p => p.Surname, f => f.Person.LastName)
                .RuleFor(p => p.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(p => p.Gender, f => (f.Person.Gender == Bogus.DataSets.Name.Gender.Male) ? Gender.Male : Gender.Female)
                .RuleFor(p => p.Earnings, f => f.Random.Int(4300, 100000))
                .Generate(count);
        }
        private List<Company> GenerateFakeCompanies(int count)
        {
            return new Bogus.Faker<Company>()
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .Generate(count);
        }
        private static List<Employment> SetRelations(IList<Person> fakePersons, IList<Company> fakeCompanies)
        {
            var key = 0;
            var rand = new Random();
            var fakeEmployments = new List<Employment>();
            foreach (var company in fakeCompanies)
            {
                var bossIndex = rand.Next(0, fakePersons.Count());
                company.Boss = fakePersons[bossIndex];
                AddNewEmployment(fakeCompanies, fakeEmployments, key++, fakePersons[bossIndex], company, Contract.EmploymentContract);
            }
            const double hasParentPropability = 0.95;
            const double hasPartnerPropability = 0.4;
            foreach (var person in fakePersons)
            {
                if (rand.NextDouble() < hasParentPropability)
                {
                    int i = 0;
                    var personIndex = rand.Next(0, fakePersons.Count());
                    for (; person == fakePersons[personIndex + i] ||
                        fakePersons[personIndex + i].Gender == Gender.Male; i++)
                    {
                        if (personIndex + i == fakePersons.Count() - 1)
                        {
                            personIndex = 0;
                            i = 0;
                        }
                    }
                    person.Mother = fakePersons[personIndex + i];
                    fakePersons[personIndex + i].Children.Add(person);
                }
                if (rand.NextDouble() < hasParentPropability)
                {
                    var personIndex = rand.Next(0, fakePersons.Count());
                    int i = 0;
                    for (; person == fakePersons[personIndex + i] ||
                        fakePersons[personIndex + i].Gender == Gender.Female; i++)
                    {
                        if (personIndex + i == fakePersons.Count() - 1)
                        {
                            personIndex = 0;
                            i = 0;
                        }
                    }
                    person.Father = fakePersons[personIndex + i];
                    fakePersons[personIndex + i].Children.Add(person);
                }
                if (person.Partner == null && rand.NextDouble() < hasPartnerPropability)
                {
                    var personIndex = rand.Next(0, fakePersons.Count());
                    int i = 0;
                    for (; fakePersons[personIndex + i].Partner != null ||
                        person == fakePersons[personIndex + i] ||
                        person.Gender == fakePersons[personIndex + i].Gender; i++)
                    {
                        if (personIndex + i == fakePersons.Count() - 1)
                        {
                            personIndex = 0;
                            i = 0;
                        }
                    }
                    person.Partner = fakePersons[personIndex + i];
                    fakePersons[personIndex + i].Partner = person;
                }
                var contractsNumber = rand.Next(0, 5);
                const double employmentContractPropability = 0.5;
                for (var i = 0; i < contractsNumber; i++)
                {
                    var companyIndex = rand.Next(0, fakeCompanies.Count());
                    if (!person.Employments.Where(e => e.Company == fakeCompanies[companyIndex]).Any())
                    {
                        AddNewEmployment(fakeCompanies, fakeEmployments, key++, person, fakeCompanies[companyIndex],
                            (rand.NextDouble() > employmentContractPropability) ? Contract.EmploymentContract : Contract.MandateContract);
                    }
                }
            }
            return fakeEmployments;
        }

        private static void AddNewEmployment(IList<Company> fakeCompanies,
            List<Employment> fakeEmployments,
            int keyId,
            Person person,
            Company company,
            Contract contract)
        {
            var newEmployment = (new Employment
            {
                Id = keyId,
                Company = company,
                Contract = contract,
                Emploee = person
            });
            person.Employments.Add(newEmployment);
            company.Employed.Add(newEmployment);
            fakeEmployments.Add(newEmployment);
        }

        public void GenerateFakeData(int personsCount, int companiesCount)
        {
            var fakePersons = GenerateFakePersons(personsCount);
            _context.AddRange(fakePersons);
            _context.SaveChanges();
            var fakeCompanies = GenerateFakeCompanies(companiesCount);
            _context.AddRange(fakeCompanies);
            _context.SaveChanges();
            var fakeEmployments = SetRelations(fakePersons, fakeCompanies);
            _context.AddRange(fakeEmployments);
            _context.SaveChanges();
        }
    }
}
