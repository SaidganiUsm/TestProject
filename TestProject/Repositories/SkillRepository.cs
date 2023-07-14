using AutoMapper;
using TestProject.Data;
using TestProject.Dto;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly OfficeDbContext _dataContext;
        private readonly IMapper _mapper;
        public SkillRepository(OfficeDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public bool CreateSkill(Skill skill)
        {
            _dataContext.Add(skill);
            return Save();
        }

        public bool DeleteSkill(Skill skill)
        {
            _dataContext.Remove(skill);
            return Save();
        }

        public ICollection<Skill> GetAll()
        {
            return _dataContext.Skills.ToList();
        }

        public Skill GetById(int id)
        {
            return _dataContext.Skills.Where(b => b.SkillId == id).FirstOrDefault();
        }

        public Skill GetByName(string name)
        {
            return _dataContext.Skills.Where(b => b.SkillName == name).FirstOrDefault();
        }

        public Skill GetSkillTrimToUpper(SkillDto SkillCreate)
        {
            return GetAll().Where(c => c.SkillName.Trim().ToUpper() == SkillCreate.SkillName.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool IsExist(int id)
        {
            return _dataContext.Skills.Where(b => b.SkillId == id).Any();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateSkill(Skill skill)
        {
            _dataContext.Update(skill);
            return Save();
        }
    }
}
