#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Globalization;

using log4net.Core;
using log4net.Util;

namespace io.vty.cswf.log
{
    public class LogImpl : log4net.Core.LogImpl, ILog
    {
        /// <summary>
        /// The fully qualified name of this declaring type not the type of any subclass.
        /// </summary>
        private readonly static Type ThisDeclaringType = typeof(LogImpl);

        private Level m_levelDebug;
        private Level m_levelInfo;
        private Level m_levelWarn;
        private Level m_levelError;
        private Level m_levelFatal;

        public LogImpl(ILogger logger) : base(logger)
        {
        }

        protected override void ReloadLevels(log4net.Repository.ILoggerRepository repository)
        {
            base.ReloadLevels(repository);
            LevelMap levelMap = repository.LevelMap;
            m_levelDebug = levelMap.LookupWithDefault(Level.Debug);
            m_levelInfo = levelMap.LookupWithDefault(Level.Info);
            m_levelWarn = levelMap.LookupWithDefault(Level.Warn);
            m_levelError = levelMap.LookupWithDefault(Level.Error);
            m_levelFatal = levelMap.LookupWithDefault(Level.Fatal);
        }

        public void D(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        public void D(Exception e, string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), e);
            }
        }

        public void D(string format, Exception e)
        {
            if (this.IsDebugEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelDebug, format, e);
            }
        }

        public void I(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        public void I(Exception e, string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), e);
            }
        }

        public void I(string format, Exception e)
        {
            if (this.IsInfoEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelInfo, format, e);
            }
        }

        public void W(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        public void W(Exception e, string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), e);
            }
        }

        public void W(string format, Exception e)
        {
            if (this.IsWarnEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelWarn, format, e);
            }
        }

        public void E(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        public void E(Exception e, string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), e);
            }
        }

        public void E(string format, Exception e)
        {
            if (this.IsErrorEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelError, format, e);
            }
        }

        public void F(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        public void F(Exception e, string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), e);
            }
        }

        public void F(string format, Exception e)
        {
            if (this.IsFatalEnabled)
            {
                Logger.Log(ThisDeclaringType, m_levelFatal, format, e);
            }
        }
    }
}

