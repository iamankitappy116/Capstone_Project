const fs = require('fs/promises');
const path = require('path');

const filePath = path.join(__dirname, '../data/books.json');

const readBooks = async () => {
    try {
        const data = await fs.readFile(filePath, 'utf8');
        return JSON.parse(data);
    } catch (error) {
        console.error('Error reading books:', error);
        return [];
    }
};

const writeBooks = async (books) => {
    try {
        await fs.writeFile(filePath, JSON.stringify(books, null, 2), 'utf8');
    } catch (error) {
        console.error('Error writing books:', error);
    }
};

module.exports = {
    readBooks,
    writeBooks
};
