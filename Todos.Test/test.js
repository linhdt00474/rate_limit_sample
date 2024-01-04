import http from 'k6/http'
import {check} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    maxRedirects: 0,
    stages: [
        { duration: '10s', target: 500 }, 
        { duration: '5s', target: 1000 }, 
        //{ duration: '5s', target: 2000 }, 
    ],
}

export default ()=>{
    const response = http.get('https://localhost:7245/weatherforecast');
    check(response, {
        'is status 200' : (r) => r.status === 200
    });
};