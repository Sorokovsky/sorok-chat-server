import { BaseEntity } from "./base.entity";
import { Column, Entity, JoinTable, ManyToMany, OneToMany } from "typeorm";
import {
  DEFAULT_AVATAR_PATH,
  DEFAULT_CHANNEL_IMAGE_PATH,
} from "@constants/default.constant";
import { UserEntity } from "@entities/user.entity";
import { MessageEntity } from "@entities/message.entity";

@Entity({ name: "channels" })
export class ChannelEntity extends BaseEntity {
  @Column()
  public name: string;

  @Column({ default: "", nullable: true })
  public description: string;

  @Column({
    name: "image_path",
    default: DEFAULT_CHANNEL_IMAGE_PATH,
    nullable: true,
  })
  public imagePath: string;

  @Column({ name: "avatar_path", default: DEFAULT_AVATAR_PATH, nullable: true })
  public avatarPath: string;

  @ManyToMany(() => UserEntity, (user) => user.channels, {
    eager: true,
  })
  @JoinTable({
    name: "channels_members",
    joinColumn: { name: "channel_id", referencedColumnName: "id" },
    inverseJoinColumn: { name: "member_id", referencedColumnName: "id" },
  })
  public members: UserEntity[];

  @OneToMany(() => MessageEntity, (message) => message.channel, {
    eager: true,
  })
  @JoinTable({
    name: "channels_messages",
    joinColumn: { name: "channel_id", referencedColumnName: "id" },
    inverseJoinColumn: { name: "message_id", referencedColumnName: "id" },
  })
  public messages: MessageEntity[];
}
