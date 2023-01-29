import { createRouter, createWebHistory } from "vue-router"

const router = createRouter({
    history: createWebHistory(),

    routes: [
        {
            path: '/home/VueSample',
            component: () => import('./pages/VueSample.vue')
        }
    ],
})

export default router
