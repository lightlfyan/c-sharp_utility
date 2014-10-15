using System;

namespace GenFsm{
    public class FsmManager{
        private State _state = null;

        public void handle(){
            this._state.handle();
        }
        public void init_state(State _s){
            _s.before_enter();
            this._state = _s;
        } 
        public void change_state(State _s){
            this._state = _s;
        } 
    }

    public abstract class State {
        public FsmManager _fm;

        protected virtual void init_state(FsmManager _fm){

        }
        public abstract void before_enter();
        public abstract void before_leave();
        public abstract void handle();

        public void change_state(State _s){
            this.before_leave();
            _s.before_enter();
            _fm.change_state(_s);
        }
    }

    public class S1: State{
        public S1(FsmManager _fm){
            this._fm  = _fm;
        }
        public override void before_enter(){
            Console.WriteLine("s1 enter");
        }
        public override void before_leave(){
            Console.WriteLine("s1 leave");
        }
        public override void handle(){
            this.change_state(new S2(this._fm));
        }
    }

     public class S2: State{
        public S2(FsmManager _fm){
            this._fm  = _fm;
        }
        public override void before_enter(){
            Console.WriteLine("s2 enter");
        }
        public override void before_leave(){
            Console.WriteLine("s2 leave");
        }
        public override void handle(){
            this.change_state(new S1(this._fm));
        }
    }

    public class Test{
        public static void Main(){
            FsmManager f = new FsmManager();
            f.init_state(new S1(f));
            f.handle();
            f.handle();
            f.handle();
            f.handle();
            f.handle();
            Console.WriteLine("ok");
        }
    }
}