#include "include.h"

bool flag=false;
int client_sockfd;

void* threadHandle(void * arg){
    char rec[1024];
    while(1){
        if(flag==true){
            memset(rec,0,1024);
            int len=recv(client_sockfd,rec,1024,0);
            if(len>0){
                cout << rec << endl;
            }else{
                cout << "Server Disconnect!" << endl;
                close(client_sockfd);
                break;
            }
        }
    }
    return 0;
}

int main(){

    struct sockaddr_in clientaddr;
    client_sockfd=socket(AF_INET,SOCK_STREAM,0);
    clientaddr.sin_addr.s_addr=inet_addr("127.0.0.1");
    clientaddr.sin_family=AF_INET;
    clientaddr.sin_port=htons(1234);
    int conret=connect(client_sockfd,(struct sockaddr *)&clientaddr,sizeof(clientaddr));
    if(conret<0){
        cout << "Connect Error" << endl;
        exit(0);
    }
    cout << "Connect!" << endl;
    char buf[50];
    char rec[1024];
    unsigned char sendbuf[50];
    memset(buf,0,50);
    memset(rec,0,1024);
    memset(sendbuf,0,50);

    pthread_t thread;
    pthread_create(&thread,NULL,threadHandle,NULL);
    pthread_detach(thread);

    while(1){
        auto ret=fgets(buf,sizeof(buf),stdin);
        if (ret!=NULL)
        {
            auto size =(short)strlen(buf);
            auto nsize=htons(size);
            memcpy(sendbuf,&nsize,sizeof(nsize));
            memcpy(sendbuf+sizeof(nsize),buf,size);
            write(client_sockfd,sendbuf,size+sizeof(nsize));
            if (!flag)
            {
                while(1){
                    memset(rec,0,1024);
                    int i=recv(client_sockfd,rec,1024,0);
                    if(i>0){
                        string str=rec;
                        int namenum=str.find("@");
                        string name=str.substr(namenum+1);
                        if(namenum>=1){
                            cout << "Login Success Welcome " << name.c_str() << "!\n";
                            flag=true;
                        }else{
                            cout << rec << endl;
                        }
                        break;
                    }else{
                        cout << "Server Disconnect!" << endl;
                    }
                }
            }   
        }
    }
    
    close(client_sockfd);
    return 0;
}