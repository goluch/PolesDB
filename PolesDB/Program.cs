using DataBase.Data;
using PolesDB.Data;

var context = new AppDbContext();
DataGenerator fakeDataGenerator = new DataGenerator(context);
fakeDataGenerator.GenerateFakeData();

