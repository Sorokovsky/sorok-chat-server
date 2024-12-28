import { BaseEntity } from "./base.entity";
import { Column, Entity } from "typeorm";

@Entity({ name: "users" })
export class UserEntity extends BaseEntity {
  @Column({ unique: true })
  public email: string;
}
