using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using Msg.Entities;
using Msg.Providers.Repository;
using Msg.Utils;

namespace Msg.Providers.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Msg.Providers.Context.EfDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;//�κ�Model Class���޸Č���ֱ�Ӹ���DB
            AutomaticMigrationDataLossAllowed = true;

        }

        /// <summary>
        /// Seeds the specified data.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(Msg.Providers.Context.EfDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //���Ĭ��Ʒ��
            var cateRep = EfRepository<CategoriesEntity, int>.Instance;
            if (!cateRep.LoadEntities().Any())
            {
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "ţ��"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "���ɼ��"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "����ʳƷ"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "��������"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "����ʳƷ"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "�����ز�"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "С������"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "С��Ʊ��"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "ѧ����Ʒ"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "С������"
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "�ջ���Ʒ",
                    IsForMarket = true
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "������Ʒ",
                    IsForMarket = true
                });
                cateRep.AddEntity(new CategoriesEntity()
                {
                    Name = "�ۺϲ�Ʒ",
                    IsForMarket = true
                });
            }



            //���Ĭ������
            var region = new RegionEntity();

            var repRegion = EfRepository<RegionEntity, int>.Instance;
            if (!repRegion.LoadEntities().Any())
            {
                region = repRegion.AddEntity(new RegionEntity()
                {
                    Name = "��ѧ��",
                    PinyinCode = "daxuecheng",
                    Schools = new Collection<SchoolEntity>() { new SchoolEntity() { Name = "�����ѧ", SchoolFirstCode = "C" } }
                });
            }


            //��ӹ���Ա�û�
            var userRep = EfRepository<UsersEntity, int>.Instance;
            if (!userRep.LoadEntities(f => f.Email == "zhangke@meisugou.com").Any())
            {
                var salt = Utils.Utils.CreateVerifyCode(8);

                var user = new UsersEntity();

                user.Email = "zhangke@meisugou.com";
                user.NickName = "����Ա";
                user.Password = Utils.Cryptography.Crypto.MD5("zhangkeAdmin" + salt);
                user.Salt = salt;
                user.UserName = "�ſƿ�";
                user.IsActive = true;
                var school = EfRepository<SchoolEntity, int>.Instance;

                user.School = school.FindById(1);

                user = userRep.AddEntity(user);

                var role =
                    EfRepository<UserRoleEntity, int>.Instance.AddEntity(new UserRoleEntity()
                    {
                        RoleName = "����Ա",
                        IsUseable = true
                    });


                var roleMap = new UserRoleMapEntity() { CreateTime = DateTime.Now, Role = role, User = user };

                EfRepository<UserRoleMapEntity, int>.Instance.AddEntity(roleMap);

                user.RoleMap = roleMap;
                userRep.UpdateEntity(user);
            }


            //����Ż�ȯ
            var couponRep = EfRepository<CouponsEntity, Guid>.Instance;
            if (!couponRep.LoadEntities().Any())
            {
                var coupon = new CouponsEntity()
                {
                    CouponName = "ע������-��50ʡ5Ԫ�ֿ��Ż�ȯ",
                    Logo = "null",
                    CouponValue = 5,
                    FullPrice = 50,
                    ReducePrice = 5,
                    Type = CouponTypeEnum.FullReduceDeduction,
                    IsUseable = true,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Parse(ConfigHelper.GetConfigString("RegistSendCouponEndTime"))

                };
                couponRep.AddEntity(coupon);
            }


        }
    }
}
