using AutoMapper;
using Azure.Core;
using backend.Dtos;
using backend.Entities;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace backend.Repositories.AuthRepo
{
    public class AuthRepo : IAuthRepo
    {
        private readonly EcommerceDBContext _context;
        private readonly PasswordHasher<Customer> _passwordHasherCustomer; 
        private readonly PasswordHasher<User> _passwordHasherUser;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthRepo(EcommerceDBContext context, PasswordHasher<Customer> passwordHasherCustomer, PasswordHasher<User> passwordHasherUser, IEmailService emailService, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _passwordHasherCustomer = passwordHasherCustomer;
            _passwordHasherUser = passwordHasherUser;
            _emailService = emailService;
            _config = configuration;
            _mapper = mapper;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Method to register a new customer
        public async Task<Customer> RegisterCustomerAsync(CustomerSignUpVM customerSignUpVM)
        {
            // Check if email already exists
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == customerSignUpVM.Email);

            if (existingCustomer != null)
            {
                return null; // Customer already exists
            }

            // Create a new customer and hash the password
            var customer = new Customer
            {
                FirstName = customerSignUpVM.FirstName,
                LastName = customerSignUpVM.LastName,
                Email = customerSignUpVM.Email,
                EmailConfirmed = false,
                HashPassword = _passwordHasherCustomer.HashPassword(null, customerSignUpVM.Password),
                EmailVerificationToken = Guid.NewGuid().ToString(),
                Phone = customerSignUpVM.Phone,
                Address = customerSignUpVM.Address,
                City = customerSignUpVM.City,
                PostalCode = customerSignUpVM.PostalCode
            };

            var newCustomer = _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Send email to the customer
            var subject = "Welcome to LTH Shop!";
            var body = $@"
        <h2>Welcome, {customer.FirstName} {customer.LastName}!</h2>
        <p>Thank you for registering at LTH Shop.</p>
        <p>Here are your details:</p>
        <ul>
            <li><b>Email:</b> {customer.Email}</li>
            <li><b>Phone:</b> {customer.Phone}</li>
            <li><b>Address:</b> {customer.Address}, {customer.City}, {customer.PostalCode}</li>
        </ul>
        <p>Please verify your email by clicking the link below:</p>
        <a href='https://lthshop-frontend.vercel.app/verify-email/{customer.EmailVerificationToken}'>
            Verify Email
        </a>
        <p>If you have any questions, feel free to contact us.</p>
        <p>Best regards,<br>LTH Shop Team</p>";

            try
            {
                await _emailService.SendEmailAsync(customer.Email, subject, body);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

            return newCustomer.Entity;
        }


        public async Task<CustomerReadDto> ValidateCustomerCredentialsAsync(string email, string password)
        {
            // Retrieve the customer by email
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                return null; 
            }

            // Verify password using the password hasher
            var result = _passwordHasherCustomer.VerifyHashedPassword(customer, customer.HashPassword, password);

            // If password is correct, return customer
            if (result == PasswordVerificationResult.Success)
            {
                var customerDto = _mapper.Map<Customer, CustomerReadDto>(customer);
                return customerDto;
            }

            return null;  
        }
        public async Task<Customer> GetCustomerByVerificationTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            // Tìm kiếm khách hàng theo EmailVerificationToken
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.EmailVerificationToken == token);

            return customer;
        }
        public async Task<User> ValidateUserCredentialsAsync(string email, string password)
        {
            // Retrieve the user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.Email == email);

            if (user == null)
            {
                return null; 
            }

            // Verify password using the password hasher
            var result = _passwordHasherUser.VerifyHashedPassword(user, user.HashPassword, password);

            // If password is correct, return customer
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;  
        }
        public async Task<bool> ChangeCustomerPasswordAsync(string email, string newPassword)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return false;

            customer.HashPassword = _passwordHasherCustomer.HashPassword(null, newPassword);  // Mã hóa mật khẩu
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}