var app = new Vue({
    el: '#app',
    data: {
        username: ""

    },
    mounted: {

    },
    methods: {
        createUser() {
            this.loading = true;
            axios.post('/Users', { username: this.username })
                .then(res => {
                    console.log(res);
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }
});