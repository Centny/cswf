package main

import (
	"fmt"
	"github.com/Centny/gwf/log"
	"github.com/Centny/gwf/netw"
	"github.com/Centny/gwf/netw/impl"
	"github.com/Centny/gwf/netw/rc"
	"github.com/Centny/gwf/pool"
	"github.com/Centny/gwf/util"
	"net/http"
	"runtime"
)

var rcs *rc.RC_Listener_m

func main() {
	runtime.GOMAXPROCS(util.CPU())
	netw.ShowLog = true
	//netw.ShowLog_C = true
	//impl.ShowLog = true
	dnh:=netw.NewDoNotH()
	dnh.ShowLog=true
	rcs = rc.NewRC_Listener_m_j(pool.BP, ":13424", dnh)
	rcs.AddToken3("abc", 1)
	rcs.AddHFunc("args_s", Args_s)
	rcs.AddHFunc("args_m", Args_m)
	rcs.AddHFunc("call_c", CallC)
	var err = rcs.Run()
	fmt.Println(err)
	http.ListenAndServe(":13425", nil)
}

func Args_s(rc *impl.RCM_Cmd) (interface{}, error) {
	log.D("run_rc_s doing Args_s->%v", string(rc.Data()))
	return rc.StrVal("val"), nil
}

func Args_m(rc *impl.RCM_Cmd) (interface{}, error) {
	log.D("run_rc_s doing Args_m->%v", util.S2Json(rc.Map))
	return rc.Map, nil
}

func CallC(rc *impl.RCM_Cmd) (interface{}, error) {
	var cid = rc.Kvs().StrVal("cid")
	log.D("run_rc_s doing call_c by cid(%v)", cid)
	var cmd = rcs.CmdC(cid)
	if cmd == nil {
		return util.Map{
			"code": -1,
			"err":  "not found",
		}, nil
	}
	var name = rc.StrVal("name")
	var args = rc.MapVal("args")
	log.D("run_rc_s call client by name(%v),args(%v)", name, util.S2Json(args))
	var res, err = cmd.Exec_m(name, args)
	log.D("run_rc_s call client result->%v", util.S2Json(res))
	return res, err
}
