<template>
    <div>
        This is an Vite App integrated view. Click: {{  clicks }}
        <input type="button" @click="increment" value="Click me" />
    </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';

const clicks = ref(0)
function increment() {
    clicks.value++
}
</script>