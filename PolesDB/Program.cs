using DataBase.Data;
using DataBase.Model;
using Microsoft.EntityFrameworkCore;
using PolesDB.Data;

static void GenerateFakeData(AppDbContext context)
{
    DataGenerator fakeDataGenerator = new DataGenerator(context);
    fakeDataGenerator.GenerateFakeData(1000,200);
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

    IQueryable<Person> poorestFamily1gen = (from p in context.Persons.TagWith("The poorest max 1 generation family, query")
                                            orderby p.Earnings + (p.Partner == null ? 0 : p.Partner.Earnings) ascending
                                            select p).Take(1);

    string poorestFamily1genQuery = poorestFamily1gen.ToQueryString();
    Person p1f = poorestFamily1gen.Single();

    IQueryable<Person> poorestFamily2gen = (from p in context.Persons.TagWith("The poorest max 2 generation family, query")
                                            orderby p.Earnings
                                                    + (p.Partner == null ? 0 : p.Partner.Earnings
                                                    + p.Children.Sum(c => c.Earnings
                                                                  + (c.Partner == null ? 0 : c.Partner.Earnings)
                                                                  + (c.Mother == null ? 0 : c.Mother.Earnings)
                                                                  + (c.Father == null ? 0 : c.Father.Earnings))) ascending
                                            select p).Take(1);

    string poorestFamily2genQuery = poorestFamily2gen.ToQueryString();
    Person p2f = poorestFamily2gen.Single();
}