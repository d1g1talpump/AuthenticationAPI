
using AutoMapper;
using static BCrypt.Net.BCrypt;
using AuthenticationAPI.Entities;
using AuthenticationAPI.Helpers;
using AuthenticationAPI.Models.Users;

namespace AuthenticationAPI.Services;
public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    User GetUserByLogin(string login);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(int id)
    {
        return GetUser(id);
    }
    
    public User GetUserByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login) ?? throw new KeyNotFoundException("User not found");;
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");

        if (_context.Users.Any(x => x.Login == model.Login))
            throw new AppException("User with the login '" + model.Login + "' already exists");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = GetUser(id);

        // validate
        if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = HashPassword(model.Password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = GetUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User GetUser(int id)
    {
        return _context.Users.Find(id) ?? throw new KeyNotFoundException("User not found");
    }
}