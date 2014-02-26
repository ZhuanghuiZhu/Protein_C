using System;
using System.Collections.Generic;
using System.Text;
using Telnet;
using Meteroi;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace Meteroi
{
    class broad
    {
        bool debug =  false;
        private Terminal shell = null;
        public string log;
        bool connect_state = false;
        private void change_state_to_disconnect()
        {
            connect_state = false;
        }
        private void change_state_to_connect()
        {
            connect_state = true;
        }
        public broad(string broad_address, string login_name, string login_pwd, string program)
        {
            string f;
            change_state_to_disconnect();
            log = "Log:\r\n";
            if (debug == true)
            {
                change_state_to_connect();
                return;
            }
            shell = new Terminal(broad_address);
            shell.Connect(); // physcial connection
            do 
			{
			    f = shell.WaitForString("Login",false,200);
			    if (f==null) 
				    break; // this little clumsy line is better to watch in the debugger
                shell.SendResponse(login_name, true);	// send username
                f = shell.WaitForString("Password");
                if (f == null) 
				    break;
                shell.SendResponse(login_pwd, true);	// send password 
                f = shell.WaitForString("$");			// bash
                if (f == null) 
				    break;
                shell.SendResponse(program, true);	// send password 
                f = shell.WaitForString("Meteroi shell>");			// program shell
                if (f == null)
                    break;
                change_state_to_connect();
            } while (false);
            log += shell.VirtualScreen.Hardcopy().TrimEnd();
            log += "\r\n";
        }
        public bool is_connect()
        {
            return connect_state;
        }
        public string send_command_get_response(string command)
        {
            string f;
            if (debug == true)
            {
                log += command;
                log += "\r\n";
                return command;
            }
            lock (this)
            {
                shell.VirtualScreen.CleanScreen(); 
                shell.SendResponse(command, true);	// send password 
                f = shell.WaitForString("Meteroi shell>",true,3);			   // program shell
                if (f == null) {
                    change_state_to_disconnect();
                    return null;
                }
            }
            log += shell.VirtualScreen.Hardcopy().TrimEnd();
            log += "\r\n";
            return shell.VirtualScreen.Hardcopy().TrimEnd();
        }
        public bool turn_off(string password)
        {
            string f;
            lock (this)
            {
                shell.VirtualScreen.CleanScreen();
                if (connect_state)
                {
                    shell.SendResponse("exit", true);	// send exit 
                    f = shell.WaitForString("$", true, 3); // program shell
                    if (f == null)
                        return false;
                }
                shell.VirtualScreen.CleanScreen();
                shell.SendResponse("sudo poweroff", true);	// send poweroff
                f = shell.WaitForString("password");
                if (f == null)
                    return false;
                shell.SendResponse(password, true);	   // send poweroff

            }
            log += shell.VirtualScreen.Hardcopy().TrimEnd();
            log += "\r\n";
            return true;
        }

        ~broad()
        { 
            
        }
    }

    class command
    { 
        //int command_state;
        //float resault;
    }

    class PCAS
    {
        static broad b = null;
        static Queue command = new Queue();

        static void command_thread()
        {
            while (true)
            {
                command.Dequeue();
                Console.WriteLine("do a command\n");
            }
        }

        static void Main(string[] args)
        {
            connect("10.235.6.197", "lophilo", "lab123");
            Thread th = new Thread(command_thread);
            th.Start();
            while (true)
            {
                Console.WriteLine(get_box_temperature());
                Console.WriteLine(get_box_moisture());
            }
        }
        public static bool connect(string broad_address, string login_name, string login_pwd)
        {
            b = new broad(broad_address, login_name, login_pwd, "~/test/thermal");
            if (b.is_connect())
                return true;
            else
                return false;
        }

        public static bool disconnect(string broad_address, string login_name, string login_pwd)
        {
            return b.turn_off(login_pwd);
        }

        public static string get_log()
        {
            return b.log;
        }

        public static float get_box_temperature()
        {
            string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
            string response;
            float resualt;
            response = b.send_command_get_response("temp 1");
            if (response == null)
                return float.NaN;
            MatchCollection mc = Regex.Matches(response, regexStr);
            if(mc.Count < 2)
                return float.NaN;
            resualt = float.Parse(mc[1].Value);
            return resualt;
        }
        public static float get_box_moisture()
        {
            string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
            string response;
            float resualt;
            response = b.send_command_get_response("moist 1");
            if (response == null)
                return float.NaN;
            MatchCollection mc = Regex.Matches(response, regexStr);
            if (mc.Count < 2)
                return float.NaN;
            resualt = float.Parse(mc[1].Value);
            return resualt;
        }
        public static float get_chip_temperature()
        {
            string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
            string response;
            float resualt;
            response = b.send_command_get_response("temp 2");
            if (response == null)
                return float.NaN;
            MatchCollection mc = Regex.Matches(response, regexStr);
            if (mc.Count < 2)
                return float.NaN;
            resualt = float.Parse(mc[1].Value);
            return resualt;
        }
        public static float get_chip_moisture()
        {
            string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
            string response;
            float resualt;
            response = b.send_command_get_response("moist 2");
            if (response == null)
                return float.NaN;
            MatchCollection mc = Regex.Matches(response, regexStr);
            if(mc.Count < 2)
                return float.NaN;
            resualt = float.Parse(mc[1].Value);
            return resualt;
        }
        public static void set_target_temperature(float target)
        {
            string com = "tempt " + target.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_target_moisture(float target)
        {
            string com = "moistt " + target.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void micoscope_x(int x)
        {
            string com = "x " + x.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void micoscope_y(int y)
        {
            string com = "y " + y.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void micoscope_z(int z)
        {
            string com = "z " + z.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void move_to_sample(uint i)
        {
            string com = "move " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void syringe_plus(uint i)
        {
            string com = "syf " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void syringe_minus(uint i)
        {
            string com = "syb " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_led(uint pwm)
        {
            string com = "led " + pwm.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_ref(uint i)
        {
            string com = "ref " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_radius(float i)
        {
            string com = "rad " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_angle(float i)
        {
            string com = "angle " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_sample(uint i)
        {
            string com = "sample " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void move_to_hole(uint i)
        {
            string com = "hole " + i.ToString();
            b.send_command_get_response(com);
            return;
        } 
         public static void set_hole_radius(float i)
        {
            string com = "holer " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_hole_sample(uint i)
        {
            string com = "holes " + i.ToString();
            b.send_command_get_response(com);
            return;
        } 
        public static void set_hole_angle(float i)
        {
            string com = "holea " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_hole_delta(float i)
        {
            string com = "holed " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void set_hole_uL(float i)
        {
            string com = "holeuL " + i.ToString();
            b.send_command_get_response(com);
            return;
        }
        public static void pannel_in()
        {
            string com = "pi ";
            b.send_command_get_response(com);
            return;
        }
        public static void pannel_out()
        {
            string com = "po ";
            b.send_command_get_response(com);
            return;
        }
        public static void set_hole_z()
        {
            string com = "holez ";
            b.send_command_get_response(com);
            return;
        }
        public static void set_view_z()
        {
            string com = "viewz ";
            b.send_command_get_response(com);
            return;
        }
        public static void microscopexy(int x, int y)
        {
            string com = "xy " + x.ToString()+' '+y.ToString();
            b.send_command_get_response(com);
            return;
        }
    }

}
