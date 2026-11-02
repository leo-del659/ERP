<template>
  <div class="page-card">
    <el-tabs v-model="tab" @tab-change="onTabChange">
      <el-tab-pane label="销售订单" name="order">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="keyword" placeholder="订单号" clearable style="width: 220px" @keyup.enter="search" />
            <el-button type="primary" @click="search">查询</el-button>
            <el-button @click="reset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openCreate">新建订单</el-button>
            <el-button @click="load">刷新</el-button>
          </div>
        </div>

        <el-table :data="rows" v-loading="loading" border stripe size="small">
          <el-table-column prop="orderNo" label="订单号" width="170" />
          <el-table-column prop="customerId" label="客户" width="180">
            <template #default="{ row }">{{ customerName(row.customerId) }}</template>
          </el-table-column>
          <el-table-column prop="materials" label="物料" min-width="220" show-overflow-tooltip>
            <template #default="{ row }">{{ row.materials || '-' }}</template>
          </el-table-column>
          <el-table-column prop="orderDate" label="订单日期" width="120" />
          <el-table-column prop="deliveryDate" label="交货日期" width="120" />
          <el-table-column prop="totalAmount" label="订单金额" width="130" align="right">
            <template #default="{ row }">¥ {{ Number(row.totalAmount ?? 0).toFixed(2) }}</template>
          </el-table-column>
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }">
              <el-tag :type="soTag(row.status)" size="small">{{ soText(row.status) }}</el-tag>
            </template>
          </el-table-column>
          <el-table-column label="操作" width="220" fixed="right" align="center">
            <template #default="{ row }">
              <el-button v-if="row.status === 0" type="primary" link @click="confirmOrder(row)">确认</el-button>
              <el-button v-if="row.status === 1" type="warning" link @click="openDeliver(row)">发货</el-button>
              <el-button v-if="row.status === 2" type="success" link @click="completeOrder(row)">完成</el-button>
            </template>
          </el-table-column>
        </el-table>

        <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />
      </el-tab-pane>

      <el-tab-pane label="销售报价" name="quote">
        <div class="toolbar">
          <div class="right" style="margin-left:auto">
            <el-button type="primary" @click="openQuote">新建报价</el-button>
            <el-button @click="qQuote.load()">刷新</el-button>
          </div>
        </div>

        <el-table :data="qQuote.rows" v-loading="qQuote.loading" border stripe size="small">
          <el-table-column prop="quoteNo" label="报价单号" width="170" />
          <el-table-column prop="customerId" label="客户" width="180">
            <template #default="{ row }">{{ customerName(row.customerId) }}</template>
          </el-table-column>
          <el-table-column prop="quoteDate" label="报价日期" width="120" />
          <el-table-column prop="priceStrategy" label="价格策略" width="110" />
          <el-table-column prop="totalAmount" label="报价金额" width="130" align="right">
            <template #default="{ row }">¥ {{ Number(row.totalAmount ?? 0).toFixed(2) }}</template>
          </el-table-column>
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }"><el-tag :type="quoteTag(row.status)" size="small">{{ quoteText(row.status) }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="240" fixed="right" align="center">
            <template #default="{ row }">
              <el-button link type="primary" @click="openQuoteDetail(row)">详情</el-button>
              <el-button v-if="row.status === 0" link type="success" @click="confirmQuote(row)">生效</el-button>
              <el-button v-if="row.status === 1" link type="warning" @click="quoteToOrder(row)">转订单</el-button>
            </template>
          </el-table-column>
        </el-table>

        <ErpPagination :total="qQuote.total" v-model:page="qQuote.page" v-model:size="qQuote.size" @change="qQuote.load()" @size-change="qQuote.search()" />
      </el-tab-pane>

      <el-tab-pane label="客户信用" name="credit">
        <div class="toolbar">
          <div class="right" style="margin-left:auto">
            <el-button type="primary" @click="openCredit()">维护额度</el-button>
            <el-button @click="qCredit.load()">刷新</el-button>
          </div>
        </div>

        <el-table :data="qCredit.rows" v-loading="qCredit.loading" border stripe size="small">
          <el-table-column prop="customerCode" label="客户编码" width="140" />
          <el-table-column prop="customerName" label="客户名称" min-width="180" />
          <el-table-column prop="creditLimit" label="信用额度" width="130" align="right">
            <template #default="{ row }">¥ {{ Number(row.creditLimit ?? 0).toFixed(2) }}</template>
          </el-table-column>
          <el-table-column prop="usedAmount" label="已用额度" width="130" align="right">
            <template #default="{ row }">¥ {{ Number(row.usedAmount ?? 0).toFixed(2) }}</template>
          </el-table-column>
          <el-table-column prop="available" label="可用额度" width="130" align="right">
            <template #default="{ row }">¥ {{ Number(row.available ?? 0).toFixed(2) }}</template>
          </el-table-column>
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }"><el-tag :type="row.status === 1 ? 'success' : 'danger'" size="small">{{ row.status === 1 ? '正常' : '冻结' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="180" fixed="right" align="center">
            <template #default="{ row }">
              <el-button link type="primary" @click="openCredit(row)">维护</el-button>
              <el-button link :type="row.status === 1 ? 'danger' : 'success'" @click="freezeCredit(row)">{{ row.status === 1 ? '冻结' : '解冻' }}</el-button>
            </template>
          </el-table-column>
        </el-table>

        <ErpPagination :total="qCredit.total" v-model:page="qCredit.page" v-model:size="qCredit.size" @change="qCredit.load()" @size-change="qCredit.search()" />
      </el-tab-pane>

      <el-tab-pane label="发货跟踪" name="delivery">
        <div class="toolbar">
          <div class="right" style="margin-left:auto">
            <el-button @click="qDelivery.load()">刷新</el-button>
          </div>
        </div>

        <el-table :data="qDelivery.rows" v-loading="qDelivery.loading" border stripe size="small">
          <el-table-column prop="deliveryNo" label="发货单号" width="170" />
          <el-table-column prop="orderId" label="订单ID" width="90" />
          <el-table-column prop="customerId" label="客户" width="180">
            <template #default="{ row }">{{ customerName(row.customerId) }}</template>
          </el-table-column>
          <el-table-column prop="deliveryDate" label="发货日期" width="120" />
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }"><el-tag :type="deliveryTag(row.status)" size="small">{{ deliveryText(row.status) }}</el-tag></template>
          </el-table-column>
          <el-table-column prop="createTime" label="创建时间" width="170">
            <template #default="{ row }">{{ row.createTime?.replace('T', ' ').slice(0, 19) ?? '-' }}</template>
          </el-table-column>
          <el-table-column label="操作" width="120" fixed="right" align="center">
            <template #default="{ row }">
              <el-button link type="primary" @click="openTrack(row)">物流轨迹</el-button>
            </template>
          </el-table-column>
        </el-table>

        <ErpPagination :total="qDelivery.total" v-model:page="qDelivery.page" v-model:size="qDelivery.size" @change="qDelivery.load()" @size-change="qDelivery.search()" />
      </el-tab-pane>
    </el-tabs>

    <el-dialog v-model="createVisible" title="新建销售订单" width="860px" destroy-on-close>
      <el-form :model="createForm" label-width="90px">
        <el-row :gutter="16">
          <el-col :span="8">
            <el-form-item label="客户" required>
              <el-select v-model="createForm.customerId" filterable placeholder="选择客户" style="width: 100%">
                <el-option v-for="c in customers" :key="c.id" :label="c.customerName" :value="c.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="订单日期" required>
              <el-date-picker v-model="createForm.orderDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="交货日期">
              <el-date-picker v-model="createForm.deliveryDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" :disabled-date="disableDeliveryDate" style="width: 100%" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <el-table :data="createForm.lines" border size="small">
        <el-table-column label="物料" min-width="260">
          <template #default="{ row }">
            <el-select v-model="row.materialId" filterable placeholder="选择物料" style="width: 100%">
              <el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" />
            </el-select>
          </template>
        </el-table-column>
        <el-table-column label="数量" width="160">
          <template #default="{ row }">
            <el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width: 100%" />
          </template>
        </el-table-column>
        <el-table-column label="单价" width="160">
          <template #default="{ row }">
            <el-input-number v-model="row.unitPrice" :min="0" :precision="2" controls-position="right" style="width: 100%" />
          </template>
        </el-table-column>
        <el-table-column label="小计" width="120" align="right">
          <template #default="{ row }">{{ (row.quantity * row.unitPrice).toFixed(2) }}</template>
        </el-table-column>
        <el-table-column label="操作" width="80" align="center">
          <template #default="{ $index }">
            <el-button type="danger" link @click="createForm.lines.splice($index, 1)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="actions">
        <el-button @click="createForm.lines.push({ materialId: undefined, quantity: 1, unitPrice: 0 })">添加明细</el-button>
        <span class="total">合计：¥ {{ createTotal.toFixed(2) }}</span>
      </div>

      <template #footer>
        <el-button @click="createVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitCreate">保存订单</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="deliverVisible" title="发货出库" width="760px" destroy-on-close>
      <el-form :model="deliverForm" label-width="90px">
        <el-row :gutter="16">
          <el-col :span="12">
            <el-form-item label="出库仓库" required>
              <el-select v-model="deliverForm.warehouseId" placeholder="选择仓库" style="width: 100%">
                <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="发货日期" required>
              <el-date-picker v-model="deliverForm.deliveryDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 100%" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <el-table :data="deliverForm.lines" border size="small">
        <el-table-column label="物料" min-width="260">
          <template #default="{ row }">
            <el-select v-model="row.materialId" filterable placeholder="选择物料" style="width: 100%">
              <el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" />
            </el-select>
          </template>
        </el-table-column>
        <el-table-column label="发货数量" width="180">
          <template #default="{ row }">
            <el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width: 100%" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="80" align="center">
          <template #default="{ $index }">
            <el-button type="danger" link @click="deliverForm.lines.splice($index, 1)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="actions">
        <el-button @click="deliverForm.lines.push({ materialId: undefined, quantity: 1 })">添加明细</el-button>
      </div>

      <template #footer>
        <el-button @click="deliverVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitDeliver">确认发货</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="quoteVisible" title="新建销售报价" width="860px" destroy-on-close>
      <el-form :model="quoteForm" label-width="90px">
        <el-row :gutter="16">
          <el-col :span="8">
            <el-form-item label="客户" required>
              <el-select v-model="quoteForm.customerId" filterable placeholder="选择客户" style="width: 100%">
                <el-option v-for="c in customers" :key="c.id" :label="c.customerName" :value="c.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="报价日期" required>
              <el-date-picker v-model="quoteForm.quoteDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="有效期至">
              <el-date-picker v-model="quoteForm.validDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" :disabled-date="disableValidDate" style="width: 100%" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="16">
          <el-col :span="8">
            <el-form-item label="价格策略">
              <el-select v-model="quoteForm.priceStrategy" style="width: 100%">
                <el-option label="标准价" value="STANDARD" />
                <el-option label="优选价" value="PREFER" />
                <el-option label="会员价" value="VIP" />
                <el-option label="协议价" value="CUSTOM" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="表头折扣">
              <el-input-number v-model="quoteForm.discountRate" :min="0" :max="1" :step="0.01" :precision="2" controls-position="right" style="width: 100%" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <el-table :data="quoteForm.lines" border size="small">
        <el-table-column label="物料" min-width="240">
          <template #default="{ row }">
            <el-select v-model="row.materialId" filterable placeholder="选择物料" style="width: 100%">
              <el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" />
            </el-select>
          </template>
        </el-table-column>
        <el-table-column label="数量" width="140">
          <template #default="{ row }"><el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width: 100%" /></template>
        </el-table-column>
        <el-table-column label="单价" width="140">
          <template #default="{ row }"><el-input-number v-model="row.unitPrice" :min="0" :precision="2" controls-position="right" style="width: 100%" /></template>
        </el-table-column>
        <el-table-column label="行折扣" width="140">
          <template #default="{ row }"><el-input-number v-model="row.discountRate" :min="0" :max="1" :step="0.01" :precision="2" controls-position="right" style="width: 100%" /></template>
        </el-table-column>
        <el-table-column label="操作" width="80" align="center">
          <template #default="{ $index }"><el-button type="danger" link @click="quoteForm.lines.splice($index, 1)">删除</el-button></template>
        </el-table-column>
      </el-table>
      <div class="actions"><el-button @click="quoteForm.lines.push({ materialId: undefined, quantity: 1, unitPrice: 0, discountRate: 1 })">添加明细</el-button></div>

      <template #footer>
        <el-button @click="quoteVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitQuote">保存报价</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="quoteDetailVisible" title="报价单详情" width="720px" destroy-on-close>
      <el-descriptions :column="2" size="small" border>
        <el-descriptions-item label="报价单号">{{ quoteDetail.quote?.quoteNo }}</el-descriptions-item>
        <el-descriptions-item label="客户">{{ customerName(quoteDetail.quote?.customerId) }}</el-descriptions-item>
        <el-descriptions-item label="报价日期">{{ quoteDetail.quote?.quoteDate }}</el-descriptions-item>
        <el-descriptions-item label="有效期至">{{ quoteDetail.quote?.validDate ?? '-' }}</el-descriptions-item>
        <el-descriptions-item label="价格策略">{{ quoteDetail.quote?.priceStrategy }}</el-descriptions-item>
        <el-descriptions-item label="状态">{{ quoteText(quoteDetail.quote?.status ?? 0) }}</el-descriptions-item>
        <el-descriptions-item label="总金额">¥ {{ Number(quoteDetail.quote?.totalAmount ?? 0).toFixed(2) }}</el-descriptions-item>
        <el-descriptions-item label="备注">{{ quoteDetail.quote?.remark ?? '-' }}</el-descriptions-item>
      </el-descriptions>
      <el-table :data="quoteDetail.details" border size="small" style="margin-top:12px">
        <el-table-column prop="materialId" label="物料" min-width="220">
          <template #default="{ row }">{{ materialName(row.materialId) }}</template>
        </el-table-column>
        <el-table-column prop="quantity" label="数量" width="100" align="right" />
        <el-table-column prop="unitPrice" label="单价" width="110" align="right" />
        <el-table-column prop="discountRate" label="折扣" width="90" align="right" />
        <el-table-column prop="amount" label="金额" width="120" align="right" />
      </el-table>
    </el-dialog>

    <el-dialog v-model="creditVisible" title="维护客户信用额度" width="480px" destroy-on-close>
      <el-form :model="creditForm" label-width="100px">
        <el-form-item label="客户" required>
          <el-select v-model="creditForm.customerId" filterable placeholder="选择客户" style="width:100%">
            <el-option v-for="c in customers" :key="c.id" :label="c.customerName" :value="c.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="信用额度">
          <el-input-number v-model="creditForm.creditLimit" :min="0" :precision="2" controls-position="right" style="width:100%" />
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="creditForm.status" style="width:100%">
            <el-option label="正常" :value="1" />
            <el-option label="冻结" :value="0" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="creditVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitCredit">保存</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="trackVisible" title="物流轨迹" width="640px" destroy-on-close>
      <el-timeline>
        <el-timeline-item v-for="(t, i) in tracks" :key="i" :type="t.trackStatus === '已签收' ? 'success' : 'primary'">
          <div><b>{{ t.trackStatus }}</b> · {{ t.carrier }} · {{ t.trackNo }}</div>
          <div style="color:#909399;font-size:12px">{{ t.location }} {{ t.trackTime?.replace('T', ' ') }}</div>
          <div v-if="t.remark" style="color:#606266;font-size:12px">{{ t.remark }}</div>
        </el-timeline-item>
      </el-timeline>
      <el-divider>登记跟踪节点</el-divider>
      <el-form :model="trackForm" label-width="90px" inline>
        <el-form-item label="承运商"><el-input v-model="trackForm.carrier" placeholder="如 顺丰" /></el-form-item>
        <el-form-item label="运单号"><el-input v-model="trackForm.trackNo" /></el-form-item>
        <el-form-item label="状态">
          <el-select v-model="trackForm.trackStatus" style="width:140px">
            <el-option label="已揽收" value="已揽收" />
            <el-option label="运输中" value="运输中" />
            <el-option label="派送中" value="派送中" />
            <el-option label="已签收" value="已签收" />
          </el-select>
        </el-form-item>
        <el-form-item label="位置"><el-input v-model="trackForm.location" /></el-form-item>
        <el-form-item label="备注"><el-input v-model="trackForm.remark" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="trackVisible = false">关闭</el-button>
        <el-button type="primary" :loading="submitting" @click="submitTrack">登记</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { ElMessage, ElNotification } from 'element-plus'
import { usePageList, fetchOptions } from '@/composables/usePage'
import { get, post } from '@/utils/request'
import { submitApproval } from '@/utils/submitApproval'
import { useNotifyStore, stockNoticeKey } from '@/stores/notify'

interface Opt { id: number; materialCode?: string; materialName?: string; customerName?: string; warehouseName?: string }

const notify = useNotifyStore()
const tab = ref('order')

const { keyword, total, rows, loading, page, size, load, search, reset } = usePageList<any>('/api/so')
const qQuote = reactive(usePageList<any>('/api/so/quote'))
const qCredit = reactive(usePageList<any>('/api/so/credit'))
const qDelivery = reactive(usePageList<any>('/api/so/delivery'))

const customers = ref<Opt[]>([])
const materials = ref<Opt[]>([])
const warehouses = ref<Opt[]>([])
const submitting = ref(false)
const today = new Date().toISOString().slice(0, 10)

function onTabChange(name: string) {
  if (name === 'quote') qQuote.load()
  else if (name === 'credit') qCredit.load()
  else if (name === 'delivery') qDelivery.load()
}

const customerName = (id: number) => customers.value.find((c) => c.id === id)?.customerName ?? id
const materialName = (id: number) => materials.value.find((m) => m.id === id)?.materialName ?? id

function soText(s: number) { return ['新建', '已确认', '已发货', '已完成'][s] ?? s }
function soTag(s: number) { return (['info', 'primary', 'warning', 'success'] as const)[s] ?? 'info' }
function quoteText(s: number) { return ['草稿', '已生效', '已转订单'][s] ?? s }
function quoteTag(s: number) { return (['info', 'success', 'warning'] as const)[s] ?? 'info' }
function deliveryText(s: number) { return ['待发运', '在途', '已签收'][s] ?? s }
function deliveryTag(s: number) { return (['info', 'primary', 'success'] as const)[s] ?? 'info' }

const createVisible = ref(false)
const deliverVisible = ref(false)
const createForm = reactive<any>({ customerId: undefined, orderDate: today, deliveryDate: '', lines: [{ materialId: undefined, quantity: 1, unitPrice: 0 }] })
const deliverForm = reactive<any>({ orderId: undefined, warehouseId: undefined, deliveryDate: today, lines: [{ materialId: undefined, quantity: 1 }] })
const createTotal = computed(() => createForm.lines.reduce((s: number, l: any) => s + (Number(l.quantity) || 0) * (Number(l.unitPrice) || 0), 0))

function openCreate() {
  createForm.customerId = undefined; createForm.orderDate = today; createForm.deliveryDate = ''; createForm.lines = [{ materialId: undefined, quantity: 1, unitPrice: 0 }]; createVisible.value = true
}
async function submitCreate() {
  if (!createForm.customerId) return ElMessage.warning('请选择客户')
  if (!createForm.lines.length || createForm.lines.some((l: any) => !l.materialId)) return ElMessage.warning('请填写完整明细')
  submitting.value = true
  try {
    await post('/api/so', { customerId: createForm.customerId, orderDate: createForm.orderDate, deliveryDate: createForm.deliveryDate || null, lines: createForm.lines.map((l: any) => ({ materialId: l.materialId, quantity: l.quantity, unitPrice: l.unitPrice })) })
    ElMessage.success('订单创建成功'); createVisible.value = false; load()
  } catch { /* 拦截器已提示 */ } finally { submitting.value = false }
}
async function confirmOrder(row: any) { try { await post(`/api/so/${row.id}/confirm`); ElMessage.success('订单已确认'); load() } catch { /* */ } }
function openDeliver(row: any) { deliverForm.orderId = row.id; deliverForm.warehouseId = undefined; deliverForm.deliveryDate = today; deliverForm.lines = [{ materialId: undefined, quantity: 1 }]; deliverVisible.value = true }
async function submitDeliver() {
  if (!deliverForm.warehouseId) return ElMessage.warning('请选择出库仓库')
  if (deliverForm.lines.some((l: any) => !l.materialId || l.quantity <= 0)) return ElMessage.warning('请填写完整发货明细')

  const stockRes: any = await get('/api/inv/stock', { page: 1, size: 500, warehouseId: deliverForm.warehouseId })
  const stockMap = new Map<number, number>()
  const nameMap = new Map<number, string>()
  for (const s of stockRes?.rows ?? []) {
    stockMap.set(s.materialId, Number(s.currentQty) || 0)
    nameMap.set(s.materialId, s.materialName)
  }
  const shortages = deliverForm.lines
    .filter((l: any) => l.materialId && l.quantity > (stockMap.get(l.materialId) ?? 0))
    .map((l: any) => ({
      materialId: l.materialId as number,
      name: nameMap.get(l.materialId as number) || `物料#${l.materialId}`,
      need: l.quantity as number,
      have: stockMap.get(l.materialId as number) ?? 0
    }))

  if (shortages.length) {
    for (const s of shortages) {
      const msg = `需求 ${s.need}，仓库仅存 ${s.have}，缺口 ${(s.need - s.have).toFixed(2)}`
      ElNotification.warning({ title: `发货提醒：${s.name} 库存不足`, message: msg, duration: 6000 })
      notify.push({ key: stockNoticeKey(String(s.materialId), deliverForm.warehouseId), type: 'stock', title: `${s.name} 库存不足，无法发货`, content: msg })
    }
    ElMessage.warning('库存不足，已阻止发货；请先入库补货或调整发货数量')
    return
  }

  submitting.value = true
  try {
    await post('/api/so/deliver', { orderId: deliverForm.orderId, warehouseId: deliverForm.warehouseId, deliveryDate: deliverForm.deliveryDate, lines: deliverForm.lines.map((l: any) => ({ materialId: l.materialId, quantity: l.quantity })) })
    ElMessage.success('发货成功，已生成应收单'); deliverVisible.value = false; load()
  } catch { /* */ } finally { submitting.value = false }
}
async function completeOrder(row: any) { try { await post(`/api/so/${row.id}/complete`); ElMessage.success('订单已完成'); load() } catch { /* */ } }

const quoteVisible = ref(false)
const quoteDetailVisible = ref(false)
const quoteForm = reactive<any>({ customerId: undefined, quoteDate: today, validDate: '', priceStrategy: 'STANDARD', discountRate: 1, lines: [{ materialId: undefined, quantity: 1, unitPrice: 0, discountRate: 1 }] })
const quoteDetail = reactive<any>({ quote: null, details: [] })

const disableDeliveryDate = (d: Date) => !!createForm.orderDate && d.getTime() < new Date(`${createForm.orderDate}T00:00:00`).getTime()
const disableValidDate = (d: Date) => !!quoteForm.quoteDate && d.getTime() < new Date(`${quoteForm.quoteDate}T00:00:00`).getTime()

function openQuote() { quoteForm.customerId = undefined; quoteForm.quoteDate = today; quoteForm.validDate = ''; quoteForm.priceStrategy = 'STANDARD'; quoteForm.discountRate = 1; quoteForm.lines = [{ materialId: undefined, quantity: 1, unitPrice: 0, discountRate: 1 }]; quoteVisible.value = true }
async function submitQuote() {
  if (!quoteForm.customerId) return ElMessage.warning('请选择客户')
  if (!quoteForm.lines.length || quoteForm.lines.some((l: any) => !l.materialId)) return ElMessage.warning('请填写完整明细')
  submitting.value = true
  try {
    const res: any = await post('/api/so/quote', {
      customerId: quoteForm.customerId, quoteDate: quoteForm.quoteDate, validDate: quoteForm.validDate || null, priceStrategy: quoteForm.priceStrategy, discountRate: quoteForm.discountRate, remark: '',
      lines: quoteForm.lines.map((l: any) => ({ materialId: l.materialId, quantity: l.quantity, unitPrice: l.unitPrice, discountRate: l.discountRate }))
    })
    const ok = await submitApproval({ bizType: 'QUOTE', bizId: res?.id, billNo: res?.quoteNo, title: `销售报价审批 ${res?.quoteNo ?? ''}`, amount: res?.totalAmount, remark: '新建报价自动送审' })
    ElMessage.success(ok ? '报价已保存，已提交审批中心审核' : '报价已保存（审批单提交失败，请在审批流中手动提交）')
    quoteVisible.value = false; qQuote.load()
  } catch { /* */ } finally { submitting.value = false }
}
async function openQuoteDetail(row: any) { const res: any = await get(`/api/so/quote/${row.id}`); quoteDetail.quote = res.quote; quoteDetail.details = res.details || []; quoteDetailVisible.value = true }
async function confirmQuote(row: any) { try { await post(`/api/so/quote/${row.id}/confirm`); ElMessage.success('报价已生效'); qQuote.load() } catch { /* */ } }
async function quoteToOrder(row: any) {
  try {
    const res: any = await post('/api/so/quote/to-order', { quoteId: row.id, deliveryDate: null })
    const ok = await submitApproval({ bizType: 'SO', bizId: res?.id, billNo: res?.orderNo, title: `销售订单审批 ${res?.orderNo ?? ''}`, amount: res?.totalAmount, remark: '报价转销售订单自动送审' })
    ElMessage.success(ok ? '已转为销售订单，已提交审批中心审核' : '已转为销售订单（审批单提交失败，请在审批流中手动提交）')
    qQuote.load()
  } catch { /* */ }
}

const creditVisible = ref(false)
const creditForm = reactive<any>({ customerId: undefined, creditLimit: 0, status: 1 })
function openCredit(row?: any) { creditForm.customerId = row?.customerId ?? undefined; creditForm.creditLimit = row?.creditLimit ?? 0; creditForm.status = row?.status ?? 1; creditVisible.value = true }
async function submitCredit() {
  if (!creditForm.customerId) return ElMessage.warning('请选择客户')
  submitting.value = true
  try {
    const res: any = await post('/api/so/credit', { customerId: creditForm.customerId, creditLimit: creditForm.creditLimit, status: creditForm.status })
    const ok = await submitApproval({ bizType: 'CREDIT', bizId: res?.id, billNo: String(res?.customerId ?? ''), title: `客户信用额度审批 ${res?.customerId ?? ''}`, amount: res?.creditLimit, remark: '客户信用额度维护自动送审' })
    ElMessage.success(ok ? '已保存，已提交审批中心审核' : '已保存（审批单提交失败，请在审批流中手动提交）')
    creditVisible.value = false; qCredit.load()
  } catch { /* */ } finally { submitting.value = false }
}
async function freezeCredit(row: any) { try { await post(`/api/so/credit/${row.customerId}/freeze?freeze=${row.status === 1}`); ElMessage.success(row.status === 1 ? '已冻结' : '已解冻'); qCredit.load() } catch { /* */ } }

const trackVisible = ref(false)
const tracks = ref<any[]>([])
const trackForm = reactive<any>({ deliveryId: undefined, carrier: '', trackNo: '', trackStatus: '已揽收', location: '', remark: '' })
async function openTrack(row: any) { trackForm.deliveryId = row.id; trackForm.carrier = ''; trackForm.trackNo = ''; trackForm.trackStatus = '已揽收'; trackForm.location = ''; trackForm.remark = ''; tracks.value = (await get(`/api/so/track/${row.id}`)) || []; trackVisible.value = true }
async function submitTrack() {
  if (!trackForm.carrier && !trackForm.trackNo) return ElMessage.warning('请填写承运商或运单号')
  submitting.value = true
  try { await post('/api/so/track', { deliveryId: trackForm.deliveryId, carrier: trackForm.carrier, trackNo: trackForm.trackNo, trackStatus: trackForm.trackStatus, location: trackForm.location, remark: trackForm.remark }); ElMessage.success('已登记'); tracks.value = (await get(`/api/so/track/${trackForm.deliveryId}`)) || [] } catch { /* */ } finally { submitting.value = false }
}

onMounted(async () => {
  customers.value = await fetchOptions<Opt>('customer')
  materials.value = await fetchOptions<Opt>('material')
  warehouses.value = await fetchOptions<Opt>('warehouse')
  load()
})
</script>

<style scoped>
.pager { margin-top: 12px; justify-content: flex-end; }
.actions { margin-top: 12px; display: flex; align-items: center; gap: 12px; }
.total { color: #f56c6c; font-weight: 600; }
.toolbar { display: flex; justify-content: space-between; margin-bottom: 12px; }
.toolbar .left, .toolbar .right { display: flex; gap: 8px; align-items: center; }
</style>
