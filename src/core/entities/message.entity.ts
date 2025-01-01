import { BaseEntity } from "@entities/base.entity";
import { Column, Entity, JoinColumn, ManyToOne } from "typeorm";
import { UserEntity } from "@entities/user.entity";
import { ChannelEntity } from "@entities/channel.entity";

@Entity({ name: "messages" })
export class MessageEntity extends BaseEntity {
  @Column()
  public text: string;

  @ManyToOne(() => UserEntity, (user) => user.messages, { eager: true })
  @JoinColumn({ name: "author_id" })
  public author: UserEntity;

  @ManyToOne(() => ChannelEntity, (channel) => channel.messages)
  @JoinColumn({ name: "channel_id" })
  public channel: ChannelEntity;
}
