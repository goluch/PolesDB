using DataBase.Data;
using DataBase.Model;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
    context.Persons.Load(); //required - otherwise the numbers of children are incorrect
    foreach (Person p in context.Persons)
    {
        //int fc = 0;
        //foreach (Person c in p.Children)
        //{
        //    if (c.Gender == Gender.Female) fc++;
        //}
        int fc = p.Doughters.Count;
        if (max < fc)
        {
            max = fc;
            maxFemChildPer = p;
        }
    }
    //query
    IQueryable<Person> mostFemaleChildren = (from p in context.Persons.TagWith("Person with the most female children query")
                                                 // orderby p.Children.Count(c => c.Gender == Gender.Female) descending
                                             //orderby p.Children.Count(c => c.Gender.Value == "Female") descending
                                             orderby p.Doughters.Count descending
                                             select p).Take(1);
    Person mostChildrenRes = mostFemaleChildren.Single();
    //int max2 = mostChildrenRes.Children.Count(c => c.Gender == Gender.Female);
    int max2 = mostChildrenRes.Doughters.Count;
    //query string
    string mostChildrenResQuery = mostFemaleChildren.ToQueryString();
    //checking code and query results
    Debug.Assert(max == max2);
}

static void Exercise2(AppDbContext context)
{
    //code
    int contrEmplNum = 0;
    int contrTotalNum = context.Employments.Count();
    context.Employments.Load();
    foreach (Employment e in context.Employments)
    {
        if (e.Contract == Contract.EmploymentContract) contrEmplNum++;
    }
    double contrEmplRatio = (double)contrEmplNum / contrTotalNum;
    //query
    int contrEmplNumRes = context.Employments/*.AsQueryable()*/.Count(e => e.Contract.ContractType == "Employment Contract");
    int contrTotalNumRes = context.Employments/*.AsQueryable()*/.Count();
    double contrEmplRatioRes = (double)contrEmplNumRes / contrTotalNumRes;
    //checking code and query results
    Debug.Assert(contrEmplRatio == contrEmplRatioRes);
}

static void Exercise3(AppDbContext context)
{
    int poor2genFamTot = int.MaxValue;
    Person poor2genFamPer = null;
    var family2gen = new HashSet<Person>();
    context.Persons.Load();
    foreach (Person p in context.Persons)
    {
        AddFamily(family2gen, p);
        if (p.Partner != null)
        {
            AddFamily(family2gen, p.Partner);
        }
        int act2genFamTot = family2gen.Sum(p => p.Earnings);
        if (act2genFamTot < poor2genFamTot)
        {
            poor2genFamTot = act2genFamTot;
            poor2genFamPer = p;
        }
        family2gen.Clear();
    }

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

    IQueryable<Person> poorestFamily2gen = from p in context.Persons
                    orderby p.Earnings
                    + (p.Partner == null ? 0 : p.Partner.Earnings)
                    + p.Doughters.Sum(d => d.Earnings
                        + (d.Mother == null ? 0 : d.Mother.Earnings)
                        + (d.Father == null ? 0 : d.Father.Earnings)
                    )
                    + p.Sons.Sum(s => s.Earnings
                        + (s.Mother == null ? 0 : s.Mother.Earnings)
                        + (s.Father == null ? 0 : s.Father.Earnings)
                    )
                    select p;

    string poorestFamily2genQuery = poorestFamily2gen.ToQueryString();
    Person p2f = poorestFamily2gen.First();

    foreach (var p in poorestFamily2gen)
    {
        var sum = p.Earnings
            + (p.Partner?.Earnings ?? 0)
            + p.Children.Sum(c => c.Earnings
                + (c.Mother == null ? 0 : c.Mother.Earnings)
                + (c.Father == null ? 0 : c.Father.Earnings));
        Console.WriteLine($"{p.Id} = {sum}");
    }
}

static void AddFamily(HashSet<Person> family2gen, Person p)
{
    family2gen.Add(p);
    foreach (var c in p.Children)
    {
        family2gen.Add(c);
        if (c.Mother != null)
        {
            family2gen.Add(c.Mother);
        }
        if (c.Father != null)
        {
            family2gen.Add(c.Father);
        }
    }
}