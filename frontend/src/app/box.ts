export interface Box {
  guid: string
  width: number
  height: number
  depth: number
  location: string
  description: string
  created: string

  toString(): string
}
