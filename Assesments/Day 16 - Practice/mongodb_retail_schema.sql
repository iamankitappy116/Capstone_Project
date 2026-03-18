
use retail_platform;

db.products.insertMany([
{
    name: "iPhone 15",
    category: "Electronics",
    price: 79999,
    stock: 50,
    attributes: {
        brand: "Apple",
        color: "Black",
        storage: "128GB"
    },
    createdAt: new Date()
},
{
    name: "Nike Running Shoes",
    category: "Footwear",
    price: 5999,
    stock: 120,
    attributes: {
        brand: "Nike",
        color: "White",
        size: "9"
    },
    createdAt: new Date()
},
{
    name: "Wooden Study Table",
    category: "Furniture",
    price: 8999,
    stock: 30,
    attributes: {
        material: "Wood",
        color: "Brown"
    },
    createdAt: new Date()
}
]);

db.orders.insertMany([
{
   userId: ObjectId("65a111111111111111111111"),
   orderDate: new Date(),
   products: [
        {
            productId: ObjectId("65b111111111111111111111"),
            name: "iPhone 15",
            price: 79999,
            quantity: 1
        },
        {
            productId: ObjectId("65b222222222222222222222"),
            name: "Nike Running Shoes",
            price: 5999,
            quantity: 2
        }
   ],
   totalCost: 91997,
   status: "Delivered"
},
{
   userId: ObjectId("65a222222222222222222222"),
   orderDate: new Date(),
   products: [
        {
            productId: ObjectId("65b333333333333333333333"),
            name: "Wooden Study Table",
            price: 8999,
            quantity: 1
        }
   ],
   totalCost: 8999,
   status: "Processing"
}
]);

db.users.insertMany([
{
    username: "ankit123",
    email: "ankit@example.com",
    passwordHash: "$2b$10$hashedpassword123",
    role: "customer",
    createdAt: new Date()
},
{
    username: "adminUser",
    email: "admin@example.com",
    passwordHash: "$2b$10$hashedpassword456",
    role: "admin",
    createdAt: new Date()
}
]);

db.products.createIndex({ category: 1 });
db.products.createIndex({ price: 1 });
db.orders.createIndex({ userId: 1 });
db.users.createIndex({ email: 1 }, { unique: true });

db.products.find({ category: "Electronics" });
db.products.find({ price: { $lt: 10000 } });

db.orders.find({ userId: ObjectId("65a111111111111111111111") });

db.orders.find({ userId: ObjectId("65a111111111111111111111") }).sort({ orderDate: -1 });

db.users.findOne({ email: "ankit@example.com" });
