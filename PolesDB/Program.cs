using DataBase.Data;
using DataBase.Model;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using PolesDB.Data;
using System.Diagnostics;
using System.Linq;

using (var context = new AppDbContext())
{
    //GenerateFakeData(context); // only once !!!
    Exercise1(context);
    Exercise2(context);
    Exercise3(context);
}
static void GenerateFakeData(AppDbContext context)
{
    DataGenerator fakeDataGenerator = new DataGenerator(context);
    fakeDataGenerator.GenerateFakeData(1000, 200);
}

static void Exercise1(AppDbContext context)
{
    //code
    int max = 0;
    Person maxFemChildPer = null;
    foreach (Person p in context.Persons)
    {
        int fc = 0;
        foreach (Person c in p.Children)
        {
            if (c.Gender == Gender.Female) fc++;
        }
        if (max < fc)
        {
            max = fc;
            maxFemChildPer = p;
        }
    }
    //query
    IQueryable<Person> mostFemaleChildren = (from p in context.Persons.TagWith("Person with the most female children query")
                                                 // orderby p.Children.Count(c => c.Gender == Gender.Female) descending
                                             orderby p.Children.Count(c => c.Gender.Value == "Female") descending
                                             select p).Take(1);
    Person mostChildrenRes = mostFemaleChildren.Single();
    //query string
    string mostChildrenResQuery = mostFemaleChildren.ToQueryString();
    //checking code and query results
    Debug.Assert(maxFemChildPer == mostChildrenRes);
}

static void Exercise2(AppDbContext context)
{
    //code
    int contrEmplNum = 0;
    int contrTotalNum = context.Employments.Count();
    foreach (Employment e in context.Employments)
    {
        if (e.Contract == Contract.EmploymentContract) contrEmplNum++;
    }
    double contrEmplRatio = (double)contrEmplNum / contrTotalNum;
    //query
    int contrEmplNumRes = context.Employments/*.AsQueryable()*/.Count(e => e.Contract.ContractType == "Employment Contract");
    int contrTotalNumRes = context.Employments/*.AsQueryable()*/.Count();
    double contrEmplRatioRes = (double)contrEmplNumRes / contrTotalNumRes;
    //query string

    //checking code and query results
    Debug.Assert(contrEmplRatio == contrEmplRatioRes);
}

static void Exercise3(AppDbContext context)
{
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