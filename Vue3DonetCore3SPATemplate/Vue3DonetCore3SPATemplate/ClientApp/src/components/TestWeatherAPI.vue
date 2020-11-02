<template>
  <div class="">
    {{weather}}
    <!-- <button @click="increment">
      Count is: {{ state.count }}, double is: {{ state.double }}
    </button> -->
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import axios from 'axios';

export default defineComponent({
  name: 'HelloWorld',
  setup(){
    const weather: any = ref([123]);

    onMounted(()=>{
      axios.get(process.env.VUE_APP_SERVER_URL + 'WeatherForecast').then(response =>{
        weather.value = response.data
        console.log(weather)
      });
    })

    return {
      weather
    }
  }

});
</script>

