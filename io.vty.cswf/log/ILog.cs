using System;
namespace io.vty.cswf.log
{
	public interface ILog : log4net.ILog
    {
        void D(string format, params object[] args);
        void D(Exception e, string format, params object[] args);
        void D(String format, Exception e);
        //
        void I(string format, params object[] args);
        void I(Exception e, string format, params object[] args);
        void I(String format, Exception e);
        //
        void W(string format, params object[] args);
        void W(Exception e, string format, params object[] args);
        void W(String format, Exception e);
        //
        void E(string format, params object[] args);
        void E(Exception e, string format, params object[] args);
        void E(String format, Exception e);
        //
        void F(string format, params object[] args);
        void F(Exception e, string format, params object[] args);
        void F(String format, Exception e);
    }
}

