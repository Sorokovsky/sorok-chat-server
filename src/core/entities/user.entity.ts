import { BaseEntity } from "./base.entity";
import {
  Column,
  Entity,
  JoinColumn,
  JoinTable,
  ManyToMany,
  OneToMany,
} from "typeorm";
import { DEFAULT_AVATAR_PATH } from "@constants/default.constant";
import { ChannelEntity } from "@entities/channel.entity";
import { MessageEntity } from "@entities/message.entity";

@Entity({ name: "users" })
export class UserEntity extends BaseEntity {
  @Column({ unique: true })
  public email: string;

  @Column({ select: false })
  public password: string;

  @Column({ nullable: true, default: "" })
  public name: string;

  @Column({ nullable: true, default: "" })
  public surname: string;

  @Column({ name: "middle_name", nullable: false, default: "" })
  public middleName: string;

  @Column({
    name: "avatar_path",
    nullable: false,
    default: DEFAULT_AVATAR_PATH,
  })
  public avatarPath: string;

  @ManyToMany(() => ChannelEntity, (channel) => channel.members, { onDelete: "CASCADE" })
  public channels: ChannelEntity[];

  @OneToMany(() => MessageEntity, (message) => message.channel, {onDelete: "CASCADE",})
  public messages: MessageEntity[];
}
