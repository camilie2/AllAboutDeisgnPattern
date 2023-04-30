
//SingletonPatternExample1.cs
//LoadBalancer클래스를 사용한 SingleTon 설계패턴을 보여줌. 
//LoadBalancer클래스는 응용프로그램 전체에서 단일 인스턴스만 생성되고 사용되도록함. 
//여러개의 인스턴스가 존재할때, 원하지 않는 동작이나 일관성 없는 상태가 발생할 수 있는 서버 관리와 같은
//시나리오에서 유용
//2개의 class [SingletonPatternExample1 & LoadBalancer ] 

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SingletonPatternExample
{
    public class SingletonPatternExample1 : MonoBehaivour
    {
        void Start()
        {

            //LoadBalancer 클래스의 GetLoadBalancer()메서드를 
            //4번 호출하여 클래스의 인스턴스 4개를 생성 
            //그러나 LoadBalancer 클래스는 Singleton으로 구현되므로
            //4개의 인스턴스(b1,b2,b3,b4) 모두 실제로 동일한 인스턴스를 참고
            //GetLoadBalancer()메서드를 사용하면 LoadBalancer 클래스의 인스턴스가
            //하나만 생성되기 때문이다. 
            LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
            LoadBalancer b4 = LoadBalancer.GetLoadBalancer();

            //Same instance?

           if(b1 == b2 && b2 == b3 && b3 =- b4)
            {
                Debug.Log("Same instance\n");

            }

            //Load balance 15 server requests

            LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
            for(int i = 0; i<15; i++)
            {
                string server = balancer.Server;
                Debug.Log("Dispatch Request to: " + server);

            }
           
        }
    }
    //싱글턴 구현 
    //서버 이름 _servers 와 난수 생성기 _random의 목록을 유지관리한다. 
    class LoadBalancer
    {
        private static LoadBalancer _instance;
        private List<string> _servers = new List<string>();
        private System.Random _random = new System.Random();

        //Lock synchronization object
        private static object syncLock = new object();

        //Constructor(protected)
        protected LoadBalancer()
        {
            //List of available servers
            _servers.Add("Server1");
            _servers.Add("Server2");
            _servers.Add("Server3");
            _servers.Add("Server4");
            _servers.Add("Server5");

        }
        //밑 함수가 가장 중요한 부분 
        //다중 스레드 환경에서도 LoadBalancer 클래스의 인스턴스가 하나만
        //생성되도록 이중으로 확인된 잠금을 사용한다.
        //서버 속성은 사용가능한 서버 목록에서 임의로 선택한 서버를 반환한다. 
        public static LoadBalancer GetLoadBalancer()
        {
            if(_instance == null)
            {
                lock(syncLock)
                {
                    if(_instance == null)
                    {
                        _instance = new LoadBalancer();
                    }
                }
            }
        }
        //syncLock 개체에 대한 잠금을획득하여 한번에 하나의 스레드만 코드의 이 섹션에 들어갈 수
        //있다. 이는 멀티 스레드 애플맄이션에서 레이스상태를 방지하는데 중요하다. 
        //잠금내부에서 _instance가 null인지 한번더 확인함으로써 더블체크잠금이다. 
        //결과적으로, 싱글턴 패턴에 의해 입증된 바와 같이 4개 변수 b1b2b3b4 모두 LoadBalancer
        //클래스의 동일한 인스턴스가 할당된다. 
        return _instance
    }

    //Simeple, but effective random load balancer
    public string Server
    {
        get
        {
            int r = _random.Next(_servers.Count);
            return _servers[r].ToString();

        }
    }
}

//What is RaceCondition?
//레이스 조건은 스레드가 실행되도록 예약된 순서와 같이 이벤트의 상대적인 타이밍에 따라
//프로그램의 동작이 달라지는 상황이다.
//멀티스레드 애플리케이션에서 두개 이상의 스레드가 공유데이터에 동시에 액세스할때
//경합조건이 발생하여 예쌍치 못한 동작이나 잘못된 결과가 발생한다.
