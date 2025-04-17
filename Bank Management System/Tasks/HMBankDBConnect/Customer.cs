using System;
using System.Text.RegularExpressions;

namespace HMBankDBConnect
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _emailAddress;
        private string _phoneNumber;
        public string Address { get; set; }

        // Default constructor
        public Customer() { }

        // Overloaded constructor
        public Customer(int customerId, string firstName, string lastName, string emailAddress, string phoneNumber, string address)
        {
            CustomerID = customerId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        // Email validation property
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (IsValidEmail(value))
                    _emailAddress = value;
                else
                    throw new ArgumentException("Invalid email address format.");
            }
        }

        // Phone number validation property
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (IsValidPhoneNumber(value))
                    _phoneNumber = value;
                else
                    throw new ArgumentException("Phone number must be 10 digits.");
            }
        }

        // Email validation method
        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        // Phone number validation method
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _);
        }

        // Method to print all customer information
        public void PrintCustomerInfo()
        {
            Console.WriteLine($"Customer ID: {CustomerID}");
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Email: {EmailAddress}");
            Console.WriteLine($"Phone: {PhoneNumber}");
            Console.WriteLine($"Address: {Address}");
        }
    }
}






