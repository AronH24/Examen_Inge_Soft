<template>
  <div id="app">
    <VendingMachine />
  </div>
</template>

<script>
import VendingMachine from './components/VendingMachine.vue'

export default {
  name: 'App',
  components: {
    VendingMachine
  }
}
</script>

<style>
#app {
  font-family: Garamond, Helvetica, Arial;
  text-align: center;
  color: #2c3e50;
  margin-top: 70px;
}
</style>