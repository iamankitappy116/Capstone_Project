interface Contact {
    id: number;
    name: string;
    email: string;
    phone: string;
}

class ContactManager {
    private contacts: Contact[] = [];

    addContact(contact: Contact): void {
        this.contacts.push(contact);
        console.log(`Contact "${contact.name}" added successfully.`);
    }

    viewContacts(): Contact[] {
        return this.contacts;
    }

    modifyContact(id: number, updatedContact: Partial<Contact>): void {
        const index = this.contacts.findIndex(c => c.id === id);
        if (index === -1) {
            console.error(`Contact with id ${id} does not exist.`);
            return;
        }
        this.contacts[index] = { ...this.contacts[index], ...updatedContact };
        console.log(`Contact with id ${id} updated successfully.`);
    }

    deleteContact(id: number): void {
        const index = this.contacts.findIndex(c => c.id === id);
        if (index === -1) {
            console.error(`Contact with id ${id} does not exist.`);
            return;
        }
        this.contacts.splice(index, 1);
        console.log(`Contact with id ${id} deleted successfully.`);
    }
}

const manager = new ContactManager();

manager.addContact({ id: 1, name: "Alice", email: "alice@example.com", phone: "9876543210" });
manager.addContact({ id: 2, name: "Bob", email: "bob@example.com", phone: "9123456789" });

console.log("\nAll Contacts:");
console.log(manager.viewContacts());

manager.modifyContact(1, { email: "alice@newmail.com" });

console.log("\nAfter modifying Alice:");
console.log(manager.viewContacts());

manager.deleteContact(2);

console.log("\nAfter deleting Bob:");
console.log(manager.viewContacts());

manager.modifyContact(99, { name: "Ghost" });
manager.deleteContact(99);
