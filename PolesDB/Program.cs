using DataBase.Data;
using DataBase.Model;
using Microsoft.EntityFrameworkCore;
using PolesDB.Data;

static void GenerateFakeData(AppDbContext context)
{
    DataGenerator fakeDataGenerator = new DataGenerator(context);
    fakeDataGenerator.GenerateFakeData(10000,100);
}

using (var context = new AppDbContext())
{
    //GenerateFakeData(context); // only once !!!

    var mostChildren = (from p in context.Persons.TagWith("Person with the most children query")
                         orderby p.Children.Count descending
                         select p).Take(1);
    string query = mostChildren.ToQueryString();

    // To niestety niedziała :(
    //int mostChildren2 = context.Persons.Max(p => p.Children.Count);

    var mostFemaleChildren = (from p in context.Persons.TagWith("Person with the most children query")
                        orderby p.Children.Count descending
                        select p).Take(1);
}

