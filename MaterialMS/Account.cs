﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialMS
{
    class Account
    {
        private static volatile Account account;

        // 锁
        private static readonly object lockObject = new object();

        // 私有构造方法
        private Account() { }

        private static User mUser;

        public static Account Instance
        {
            get
            {
                if (null == account)
                {
                    lock (lockObject)
                    {
                        if (null == account)
                        {
                            account = new Account();
                        }
                    }

                }
                return account;
            }
        }

        // 将登录成功的user信息保存下来
        public void Login(User u)
        {
            mUser = u;
        }

        // 程序退出时，需要清除保存的用户信息
        public void Logout()
        {
            if (mUser != null)
            {
                mUser = null;
            }
        }

        // 获取当前登录用的的信息
        public User GetUser()
        {
            return mUser;
        }

        // 判断当前的登录状态
        public bool IsLogin()
        {
            return mUser != null && !string.IsNullOrEmpty(mUser.employee_id)
                                 && !string.IsNullOrEmpty(mUser.password);
        }
    }

}

