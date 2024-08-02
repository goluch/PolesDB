using DataBase.Data;
using Microsoft.EntityFrameworkCore;
using PolesDB.Data;

var context = new AppDbContext();
DataGenerator fakeDataGenerator = new DataGenerator(context);
fakeDataGenerator.GenerateFakeData();
