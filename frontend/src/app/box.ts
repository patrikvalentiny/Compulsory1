import {Material} from "./material";

export interface Box {
    guid: string
    title: string
    width: number
    height: number
    depth: number
    location: string
    description: string
    created: string
    quantity: number
    material: Material
}
