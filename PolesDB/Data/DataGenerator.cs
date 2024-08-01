﻿using DataBase.Data;
using DataBase.Model;
using Domain.Common;

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
        private IList<Person> GenerateFakePersons()
        {
            return new Bogus.Faker<Person>()
                .RuleFor(p => p.Forename, f => f.Person.FirstName)
                .RuleFor(p => p.Surname, f => f.Person.LastName)
                .RuleFor(p => p.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                .RuleFor(p => p.Earnings, f => f.Random.Int(4300, 100000))
                .Generate(100);
        }
        private List<Company> GenerateFakeCompanies()
        {
            return new Bogus.Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName("az"))
                .Generate(100);
        }
        private List<Employment> GenerateFakeContracts()
        {
            throw new NotImplementedException();
        }
        private static List<Employment> SetRelations(IList<Person> fakePersons, IList<Company> fakeCompanies)
        {
            var rand = new Random();
            var fakeEmployments = new List<Employment>();
            foreach (var company in fakeCompanies)
            {
                var bossIndex = rand.Next(0, fakePersons.Count());
                AddNewEmployment(fakeCompanies, fakeEmployments, fakePersons[bossIndex], company, Contract.EmploymentContract);
            }
            const double hasParentPropability = 0.9;
            const double hasPartnerPropability = 0.6;
            foreach (var person in fakePersons)
            {
                if (rand.NextDouble() < hasParentPropability)
                {
                    var personIndex = rand.Next(0, fakePersons.Count());
                    person.Mother = fakePersons[personIndex];
                }
                if (rand.NextDouble() < hasParentPropability)
                {
                    var itemIndex = rand.Next(0, fakePersons.Count());
                    person.Father = fakePersons[itemIndex];
                }
                if (rand.NextDouble() < hasPartnerPropability)
                {
                    var itemIndex = rand.Next(0, fakePersons.Count());
                    int i = 0;
                    while (person.Gender == fakePersons[itemIndex].Gender)
                    {
                        person.Partner = fakePersons[itemIndex + i];
                        i++;
                        if (itemIndex == fakePersons.Count()) break;
                    }
                }
                var contractsNumber = rand.Next(0, 4);
                const double employmentContractPropability = 0.5;
                for (var i = 0; i < contractsNumber; i++)
                {
                    var companyIndex = rand.Next(0, fakeCompanies.Count());
                    if (person.Employments.Where(e => e.Company == fakeCompanies[companyIndex]).First() == null)
                    {
                        AddNewEmployment(fakeCompanies, fakeEmployments, person, fakeCompanies[companyIndex],
                            (rand.Next(0, 1) > employmentContractPropability) ? Contract.EmploymentContract : Contract.MandateContract);
                    }
                }
            }
            return fakeEmployments;
        }

        private static void AddNewEmployment(IList<Company> fakeCompanies,
            List<Employment> fakeEmployments,
            Person person,
            Company company,
            string contract)
        {
            var newEmployment = (new Employment
            {
                Company = company,
                Contract = new Contract(contract),
                Emploee = person
            });
            person.Employments.Add(newEmployment);
            company.Employments.Add(newEmployment);
            fakeEmployments.Add(newEmployment);
        }

        public void GenerateFakeData()
        {
            var fakePersons = GenerateFakePersons();
            var fakeCompanies = GenerateFakeCompanies();
            var fakeEmployments = SetRelations(fakePersons, fakeCompanies);
        }
    }
}