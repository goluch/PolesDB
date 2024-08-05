using DataBase.Data;
using DataBase.Model;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PolesDB.Data;

static void GenerateFakeData(AppDbContext context)
{
    DataGenerator fakeDataGenerator = new DataGenerator(context);
    fakeDataGenerator.GenerateFakeData(10000,100);
}

using (var context = new AppDbContext())
{
    //GenerateFakeData(context); // only once !!!

    IQueryable<Person> mostChildren = (from p in context.Persons.TagWith("Person with the most children query")
                         orderby p.Children.Count descending
                         select p).Take(1);
    string query = mostChildren.ToQueryString();

    // To niestety niedziała :(
    //int mostChildren2 = context.Persons.Max(p => p.Children.Count);

    IQueryable<Person> mostFemaleChildren = (from p in context.Persons.TagWith("Person with the most female children query")
                        // orderby p.Children.Count(c => c.Gender == Gender.Female) descending
                        orderby p.Children.Count(c => c.Gender.Value == "Female") descending
                        select p).Take(1);
    string query2 = mostFemaleChildren.ToQueryString();

    int employmentContractCount = context.Employments/*.AsQueryable()*/.Count(e => e.Contract.ContractType == "Employment Contract");

    IQueryable<Person> poorestPerson = (from p in context.Persons.TagWith("The poorest person, query")
                                        orderby p.Earnings ascending
                                        select p).Take(1);
    string poorestPersonQuery = poorestPerson.ToQueryString();
    Person pp = poorestPerson.First();

    IQueryable<Person> poorestFamily = (from p in context.Persons.TagWith("The poorest max 1 generation family, query")
                                        orderby p.Earnings + (p.Partner == null ? 0 : p.Partner.Earnings) ascending
                                        select p).Take(1);

    string poorestFamilyQuery = poorestFamily.ToQueryString();
    Person pf = poorestFamily.Single();
}
